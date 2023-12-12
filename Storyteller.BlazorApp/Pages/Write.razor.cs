using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Storyteller.BlazorApp.Pages;

public sealed partial class Write
{
    private ElementReference _containerRef;
    private int _currentSentenceStart;
    private bool _isSentenceComplete;

    private string AllText { get; set; } = "";

    [Inject]
    public required ClipboardService Clipboard { get; set; }

    private string CurrentSentence =>
        _currentSentenceStart < 0 ? "" : AllText[_currentSentenceStart..];

    [Inject]
    public required ILocalStorageService LocalStorage { get; set; }

    private string PreviousSentences =>
        _currentSentenceStart < 0 ? AllText : AllText[.._currentSentenceStart];

    private async Task OnContainerBlurred() => await _containerRef.FocusAsync();

    protected override async Task OnAfterRenderAsync(bool isFirstRender)
    {
        if (isFirstRender)
            await _containerRef.FocusAsync();
    }

    protected override async Task OnInitializedAsync()
    {
        AllText = await LocalStorage.GetItemAsStringAsync("text") ?? "";

        if (AllText.Length > 0 && AllText[^1] != ' ')
            AllText += ' ';

        _currentSentenceStart = AllText.Length;
    }

    private async Task OnKeyDown(KeyboardEventArgs args)
    {
        switch (args.Key)
        {
            case "!"
            or "?"
            or "." when args is { CtrlKey: false, AltKey: false, MetaKey: false }:
                Punctuate();
                break;
            case "Enter":
                NewLine();
                break;
            case "Backspace":
                Backspace();
                break;
            case "Delete"
                when args is { CtrlKey: true, AltKey: false, MetaKey: false, ShiftKey: false }:
                Clear();
                break;
            case " " when args is { CtrlKey: false, AltKey: false, MetaKey: false }:
                HandleSpace();
                break;
            case "c" when args is { CtrlKey: true, AltKey: false, MetaKey: false, ShiftKey: false }:
                await Copy();
                break;
            case "v" when args is { CtrlKey: true, AltKey: false, MetaKey: false, ShiftKey: false }:
                await Paste();
                break;
            default:
            {
                if (
                    args.Key.Length != 1
                    || args is not { CtrlKey: false, AltKey: false, MetaKey: false }
                )
                    return;

                if (_isSentenceComplete)
                {
                    AllText += ' ';
                    StartNewSentence();
                }

                AllText += args.Key;
                break;
            }
        }

        await LocalStorage.SetItemAsStringAsync("text", AllText);
        return;

        void Backspace()
        {
            if (
                _currentSentenceStart >= AllText.Length
                && (AllText.Length == 0 || AllText[^1] != '\n')
            )
                return;

            AllText = AllText[..^1];
            _isSentenceComplete = false;
            _currentSentenceStart = Math.Min(_currentSentenceStart, AllText.Length);
        }

        void Clear()
        {
            _currentSentenceStart = 0;
            _isSentenceComplete = false;
            AllText = "";
        }

        ValueTask Copy() => Clipboard.WriteTextAsync(AllText);

        void HandleSpace()
        {
            if (!_isSentenceComplete && _currentSentenceStart < AllText.Length)
                AllText += ' ';
        }

        bool IsSentenceTerminator(char c) => c is '!' or '?' or '.' or '\n';

        void NewLine()
        {
            AllText = AllText.TrimEnd() + '\n';
            StartNewSentence();
        }

        async ValueTask Paste()
        {
            AllText += await Clipboard.ReadTextAsync();

            for (var i = AllText.Length - 1; i > 0; i--)
            {
                if (!IsSentenceTerminator(AllText[i]))
                    continue;

                _currentSentenceStart =
                    i
                    + (
                        i < AllText.Length - 1 && (AllText[i + 1] == ' ' || AllText[i + 1] == '\n')
                            ? 2
                            : 1
                    );
                return;
            }

            _currentSentenceStart = 0;
        }

        void Punctuate()
        {
            AllText += args.Key;
            _isSentenceComplete = true;
        }

        void StartNewSentence()
        {
            _isSentenceComplete = false;
            _currentSentenceStart = AllText.Length;
        }
    }
}

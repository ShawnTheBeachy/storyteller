using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Storyteller.BlazorApp.KeyHandlers;

namespace Storyteller.BlazorApp.Pages;

public sealed partial class Write
{
    private ElementReference _containerRef;
    private int _currentSentenceStart;
    private readonly List<KeyHandlerBase> _keyHandlers = new();
    private const string StorageKey = "text";

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
        AllText = await LocalStorage.GetItemAsStringAsync(StorageKey) ?? "";
        _currentSentenceStart = AllText.FindLastSentenceStart();
        RegisterKeyHandlers();
    }

    private async Task OnKeyDown(KeyboardEventArgs args)
    {
        foreach (var handler in _keyHandlers)
        {
            var handled = handler is AsyncKeyHandlerBase asyncHandler
                ? await asyncHandler.OnKeyAsync(args)
                : handler.OnKey(args);

            if (handled)
                break;
        }

        await LocalStorage.SetItemAsStringAsync(StorageKey, AllText);
    }

    private void RegisterKeyHandlers()
    {
        var args = new KeyHandlerArgs(
            () => AllText,
            text => AllText = text,
            () => _currentSentenceStart,
            index => _currentSentenceStart = index
        );

        var handlers = new KeyHandlerBase[]
        {
            new BackspaceHandler(args),
            new ClearAllTextHandler(args),
            new CopyHandler(Clipboard, args),
            new NewLineHandler(args),
            new PasteHandler(Clipboard, args),
            new PunctuationHandler(args),
            new SpaceHandler(args),
            new DefaultKeyHandler(args)
        };

        foreach (var handler in handlers)
            _keyHandlers.Add(handler);
    }
}

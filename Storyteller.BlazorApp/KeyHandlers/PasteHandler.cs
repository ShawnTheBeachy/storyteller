using Microsoft.AspNetCore.Components.Web;

namespace Storyteller.BlazorApp.KeyHandlers;

internal sealed class PasteHandler : AsyncKeyHandlerBase
{
    private readonly ClipboardService _clipboard;

    public PasteHandler(ClipboardService clipboard, KeyHandlerArgs args)
        : base(args)
    {
        _clipboard = clipboard;
    }

    public override async ValueTask<bool> OnKeyAsync(KeyboardEventArgs args)
    {
        if (args is not { CtrlKey: true, AltKey: false, MetaKey: false, Key: "v", ShiftKey: false })
            return false;

        Text += await _clipboard.ReadTextAsync();
        CurrentSentenceStart = Text.FindLastSentenceStart();
        return true;
    }
}

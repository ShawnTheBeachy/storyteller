using Microsoft.AspNetCore.Components.Web;

namespace Storyteller.BlazorApp.KeyHandlers;

internal sealed class CopyHandler : AsyncKeyHandlerBase
{
    private readonly ClipboardService _clipboard;

    public CopyHandler(ClipboardService clipboard, KeyHandlerArgs args)
        : base(args)
    {
        _clipboard = clipboard;
    }

    public override async ValueTask<bool> OnKeyAsync(KeyboardEventArgs args)
    {
        if (args is not { CtrlKey: true, AltKey: false, MetaKey: false, Key: "c", ShiftKey: false })
            return false;

        await _clipboard.WriteTextAsync(Text);
        return true;
    }
}

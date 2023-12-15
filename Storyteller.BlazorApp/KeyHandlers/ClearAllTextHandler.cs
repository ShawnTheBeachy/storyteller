using Microsoft.AspNetCore.Components.Web;

namespace Storyteller.BlazorApp.KeyHandlers;

internal sealed class ClearAllTextHandler : KeyHandlerBase
{
    public ClearAllTextHandler(KeyHandlerArgs args)
        : base(args) { }

    public override bool OnKey(KeyboardEventArgs args)
    {
        if (
            args
            is not { CtrlKey: true, AltKey: false, MetaKey: false, Key: "Delete", ShiftKey: false }
        )
            return false;

        CurrentSentenceStart = 0;
        Text = "";
        return true;
    }
}

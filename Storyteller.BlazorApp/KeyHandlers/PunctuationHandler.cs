using Microsoft.AspNetCore.Components.Web;

namespace Storyteller.BlazorApp.KeyHandlers;

internal sealed class PunctuationHandler : KeyHandlerBase
{
    public PunctuationHandler(KeyHandlerArgs args)
        : base(args) { }

    public override bool OnKey(KeyboardEventArgs args)
    {
        if (
            args.Key.Length != 1
            || !args.Key[0].IsSentenceTerminator()
            || args is not { CtrlKey: false, AltKey: false, MetaKey: false }
        )
            return false;

        Text += args.Key;
        return true;
    }
}

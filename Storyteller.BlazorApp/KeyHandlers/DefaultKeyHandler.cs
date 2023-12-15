using Microsoft.AspNetCore.Components.Web;

namespace Storyteller.BlazorApp.KeyHandlers;

internal sealed class DefaultKeyHandler : KeyHandlerBase
{
    public DefaultKeyHandler(KeyHandlerArgs args)
        : base(args) { }

    public override bool OnKey(KeyboardEventArgs args)
    {
        if (args.Key.Length != 1 || args is not { CtrlKey: false, AltKey: false, MetaKey: false })
            return false;

        if (IsSentenceComplete)
        {
            if (Text[^1] != ' ')
                Text += ' ';

            CurrentSentenceStart = Text.Length;
        }

        Text += args.Key;
        return true;
    }
}

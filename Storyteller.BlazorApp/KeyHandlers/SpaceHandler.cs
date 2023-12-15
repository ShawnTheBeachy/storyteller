using Microsoft.AspNetCore.Components.Web;

namespace Storyteller.BlazorApp.KeyHandlers;

internal sealed class SpaceHandler : KeyHandlerBase
{
    public SpaceHandler(KeyHandlerArgs args)
        : base(args) { }

    public override bool OnKey(KeyboardEventArgs args)
    {
        if (args is not { CtrlKey: false, AltKey: false, Key: " ", MetaKey: false })
            return false;

        if (!IsSentenceComplete && !IsAtSentenceStart())
            Text += ' ';

        return true;

        bool IsAtSentenceStart() => CurrentSentenceStart >= Text.Length;
    }
}

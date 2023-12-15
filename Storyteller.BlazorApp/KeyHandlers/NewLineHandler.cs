using Microsoft.AspNetCore.Components.Web;

namespace Storyteller.BlazorApp.KeyHandlers;

internal sealed class NewLineHandler : KeyHandlerBase
{
    public NewLineHandler(KeyHandlerArgs args)
        : base(args) { }

    public override bool OnKey(KeyboardEventArgs args)
    {
        if (args is not { Key: "Enter" })
            return false;

        Text = Text.TrimEnd() + '\n';
        CurrentSentenceStart = Text.Length;
        return true;
    }
}

using Microsoft.AspNetCore.Components.Web;

namespace Storyteller.BlazorApp.KeyHandlers;

internal sealed class BackspaceHandler : KeyHandlerBase
{
    public BackspaceHandler(KeyHandlerArgs args)
        : base(args) { }

    public override bool OnKey(KeyboardEventArgs args)
    {
        if (args is not { Key: "Backspace" })
            return false;

        if (IsAtSentenceStart() && (IsTextEmpty() || !IsLastCharacterNewLine()))
            return true;

        Text = Text[..^1];
        CurrentSentenceStart = Math.Min(CurrentSentenceStart, Text.Length);
        return true;

        bool IsAtSentenceStart() => CurrentSentenceStart >= Text.Length;
        bool IsLastCharacterNewLine() => Text[^1] == '\n';
        bool IsTextEmpty() => Text.Length == 0;
    }
}

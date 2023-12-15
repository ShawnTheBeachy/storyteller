using Microsoft.AspNetCore.Components.Web;

namespace Storyteller.BlazorApp.KeyHandlers;

internal abstract class KeyHandlerBase
{
    private readonly KeyHandlerArgs _args;

    protected KeyHandlerBase(KeyHandlerArgs args)
    {
        _args = args;
    }

    public abstract bool OnKey(KeyboardEventArgs args);

    protected int CurrentSentenceStart
    {
        get => _args.GetCurrentSentenceStart();
        set => _args.SetCurrentSentenceStart(value);
    }

    protected bool IsSentenceComplete =>
        Text.Length > 0 && Text.TrimEnd(' ')[^1].IsSentenceTerminator();

    protected string Text
    {
        get => _args.GetText();
        set => _args.SetText(value);
    }
}

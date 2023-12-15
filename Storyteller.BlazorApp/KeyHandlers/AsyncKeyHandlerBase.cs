using Microsoft.AspNetCore.Components.Web;

namespace Storyteller.BlazorApp.KeyHandlers;

internal abstract class AsyncKeyHandlerBase : KeyHandlerBase
{
    protected AsyncKeyHandlerBase(KeyHandlerArgs args)
        : base(args) { }

    public override bool OnKey(KeyboardEventArgs args) => throw new InvalidOperationException();

    public abstract ValueTask<bool> OnKeyAsync(KeyboardEventArgs args);
}

namespace Storyteller.BlazorApp.KeyHandlers;

internal sealed record KeyHandlerArgs(
    Func<string> GetText,
    Action<string> SetText,
    Func<int> GetCurrentSentenceStart,
    Action<int> SetCurrentSentenceStart
);

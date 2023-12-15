namespace Storyteller.BlazorApp;

internal static class CharExtensions
{
    public static bool IsSentenceTerminator(this char c) => c is '!' or '?' or '.' or '\n';
}

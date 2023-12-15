namespace Storyteller.BlazorApp;

internal static class StringExtensions
{
    public static int FindLastSentenceStart(this string input)
    {
        var index = FindLastSentenceTerminator();
        return index == -1
            ? 0
            : index + (!IsLastCharacter() && IsNextCharacterSpaceOrNewLine() ? 2 : 1);

        int FindLastSentenceTerminator()
        {
            for (var i = input.Length - 1; i > 0; i--)
            {
                if (!input[i].IsSentenceTerminator())
                    continue;

                return i;
            }

            return -1;
        }

        bool IsLastCharacter() => index >= input.Length - 1;
        bool IsNextCharacterSpaceOrNewLine() => input[index + 1] is ' ' or '\n';
    }
}

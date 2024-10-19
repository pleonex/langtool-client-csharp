namespace PleOps.LanguageTool.Client.TextCheck;

using PleOps.LanguageTool.Client.Generated.Check;

/// <summary>
/// Rule match on the input text.
/// </summary>
public record TextCheckMatch
{
    internal TextCheckMatch(CheckPostResponse_matches response)
    {
        Context = response.Context!.Text!;
        TextMatch = Context.Substring(response.Context!.Offset!.Value, response.Context!.Length!.Value);
        Sentence = response.Sentence!;
        MatchOffset = response.Offset!.Value;
        MatchLength = response.Length!.Value;
        Message = response.Message!;
        ShortMessage = response.ShortMessage!;
        Replacements = response.Replacements!.Select(x => x.Value!).ToList().AsReadOnly();
        Rule = new CheckRule(response.Rule!);
    }

    /// <summary>
    /// Gets a few words around the text match.
    /// </summary>
    public string Context { get; }

    /// <summary>
    /// Gets the sentence where the match is present.
    /// </summary>
    /// <remarks>This may NOT be the original input.</remarks>
    public string Sentence { get; }

    /// <summary>
    /// Gets the text with the detected issue.
    /// </summary>
    public string TextMatch { get; }

    /// <summary>
    /// Gets the offset of the text match in the original text input.
    /// </summary>
    /// <remarks>This offset cannot be used with the Sentence.</remarks>
    public int MatchOffset { get; }

    /// <summary>
    /// Gets the length of the text match.
    /// </summary>
    public int MatchLength { get; }

    /// <summary>
    /// Gets the issue message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets a shorter version of the message.
    /// </summary>
    public string ShortMessage { get; }

    /// <summary>
    /// Gets the suggested replacements.
    /// </summary>
    public IReadOnlyCollection<string> Replacements { get; }

    /// <summary>
    /// Gets the rule that matched.
    /// </summary>
    public CheckRule Rule { get; }
}

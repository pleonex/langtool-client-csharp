namespace PleOps.LanguageTool.Client.TextCheck;

using PleOps.LanguageTool.Client.Generated.Check;

/// <summary>
/// Response data from a text check request.
/// </summary>
public record TextCheckResult
{
    internal TextCheckResult(CheckPostResponse response, IEnumerable<CheckPostResponse_matches> matches)
    {
        SoftwareInfo = new LanguageToolSoftwareInfo(response.Software!);
        Language = new LanguageInfo(response.Language!);
        DetectedLanguage = new LanguageInfo(response.Language!.DetectedLanguage!);
        Matches = matches.Select(m => new TextCheckMatch(m)).ToList().AsReadOnly();
    }

    /// <summary>
    /// Gets the information of the software that checked the text.
    /// </summary>
    public LanguageToolSoftwareInfo SoftwareInfo { get; }

    /// <summary>
    /// Gets the language used for checking the text.
    /// </summary>
    public LanguageInfo Language { get; }

    /// <summary>
    /// Gets the automatically detected text language.
    /// </summary>
    public LanguageInfo DetectedLanguage { get; }

    /// <summary>
    /// Gets a collection of detected text issues.
    /// </summary>
    public IReadOnlyCollection<TextCheckMatch> Matches { get; }
}

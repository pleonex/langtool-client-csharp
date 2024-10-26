namespace PleOps.LanguageTool.Client.TextCheck;

using System.Collections.ObjectModel;
using PleOps.LanguageTool.Client.Generated.Check;

/// <summary>
/// Informtion of a text check rule.
/// </summary>
public record CheckRule
{
    internal CheckRule(CheckPostResponse_matches_rule response)
    {
        Category = response.Category!.Name!;
        CategoryId = response.Category!.Id!;
        Id = response.Id!;
        SubId = response.SubId;
        Description = response.Description!;
        IssueType = response.IssueType;
        Urls = response.Urls?.Select(u => u.Value!).ToList().AsReadOnly()
            ?? new ReadOnlyCollection<string>([]);
    }

    /// <summary>
    /// Gets the category ID of the rule.
    /// </summary>
    public string CategoryId { get; }

    /// <summary>
    /// Gets the name of the category.
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// Gets the a rule identifier for this language.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets an optional sub identifier of the rule, used when several rules are grouped.
    /// </summary>
    public string? SubId { get; }

    /// <summary>
    /// Gets the rule description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the <see href="http://www.w3.org/International/multilingualweb/lt/drafts/its20/its20.html#lqissue-typevalues" /> Localization Quality Issue Type.
    /// This is not defined for all languages, in which case it will always be 'Uncategorized'.
    /// </summary>
    public string? IssueType { get; }

    /// <summary>
    /// Gets an optional array of URLs with a more detailed description of the error.
    /// </summary>
    public ReadOnlyCollection<string> Urls { get; }
}

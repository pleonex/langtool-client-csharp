namespace PleOps.LanguageTool.Client.TextCheck;

using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

/// <summary>
/// Build markup structures by capturing markup with regular expressions.
/// </summary>
public class RegexMarkupBuilder
{
    private readonly Regex regex;
    private readonly IReadOnlyDictionary<Regex, string> markupMapping;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegexMarkupBuilder"/> class.
    /// </summary>
    /// <param name="regex">The regular expression for markup matches.</param>
    public RegexMarkupBuilder(Regex regex)
    {
        this.regex = regex;
        markupMapping = new ReadOnlyDictionary<Regex, string>(new Dictionary<Regex, string>());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RegexMarkupBuilder"/> class.
    /// </summary>
    /// <param name="regex">The regular expression for markup matches.</param>
    /// <param name="markupMapping">Mapping text to interpret detected markup.</param>
    public RegexMarkupBuilder(Regex regex, IReadOnlyDictionary<Regex, string> markupMapping)
    {
        this.regex = regex;
        this.markupMapping = markupMapping;
    }

    /// <summary>
    /// Build a markup text by splitting the input text according to the regular expression.
    /// </summary>
    /// <param name="text">The input text to divide.</param>
    /// <returns>The markup text structure.</returns>
    public MarkupText Build(string text)
    {
        string[] segments = regex.Split(text);

        var result = new MarkupText();

        bool nextMarkup = false;
        foreach (string segment in segments) {
            // It's empty when there is a match at the beginning, end or
            // two matches consecutive.
            if (!string.IsNullOrEmpty(segment)) {
                MarkupAnnotation annotation;
                if (nextMarkup) {
                    string? mapping = FindMarkupMapping(segment);
                    annotation = MarkupAnnotation.CreateMarkup(segment, mapping);
                } else {
                    annotation = MarkupAnnotation.CreateText(segment);
                }

                result.Annotation.Add(annotation);
            }

            nextMarkup = !nextMarkup;
        }

        return result;
    }

    private string? FindMarkupMapping(string markup)
    {
        return markupMapping
            .FirstOrDefault(e => e.Key.IsMatch(markup))
            .Value;
    }
}

namespace PleOps.LanguageTool.Client.TextCheck;

using System.Text.RegularExpressions;

/// <summary>
/// Build markup structures by capturing markup with regular expressions.
/// </summary>
public class RegexMarkupBuilder
{
    private readonly Regex regex;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegexMarkupBuilder"/> class.
    /// </summary>
    /// <param name="regex">The regular expression for markup matches.</param>
    public RegexMarkupBuilder(Regex regex)
    {
        this.regex = regex;
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
                var annotation = nextMarkup
                    ? MarkupAnnotation.CreateMarkup(segment)
                    : MarkupAnnotation.CreateText(segment);
                result.Annotation.Add(annotation);
            }

            nextMarkup = !nextMarkup;
        }

        return result;
    }
}

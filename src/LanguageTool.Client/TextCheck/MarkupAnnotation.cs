namespace PleOps.LanguageTool.Client.TextCheck;

using System.Text.Json.Serialization;

/// <summary>
/// Represents a segment on a markup text.
/// </summary>
public class MarkupAnnotation
{
    /// <summary>
    /// Gets a value indicating whether the annotation is markup or plain text.
    /// </summary>
    [JsonIgnore]
    public bool IsMarkup { get; private set; }

    /// <summary>
    /// Gets the annotation text.
    /// </summary>
    [JsonIgnore]
    public string Value => IsMarkup ? Markup! : Text!;

    /// <summary>
    /// Gets an optional text representation of the markup text.
    /// </summary>
    [JsonPropertyName("interpretAs")]
    [JsonPropertyOrder(2)]
    public string? InterpretMarkupAs { get; private set; }

    [JsonInclude]
    internal string? Text { get; private set; }

    [JsonInclude]
    internal string? Markup { get; private set; }

    /// <summary>
    /// Create a segment with markup content.
    /// </summary>
    /// <param name="markup">The markup content.</param>
    /// <returns>New annotation.</returns>
    public static MarkupAnnotation CreateMarkup(string markup)
    {
        return new MarkupAnnotation { IsMarkup = true, Markup = markup };
    }

    /// <summary>
    /// Create a segment with markup content and text interpretation.
    /// </summary>
    /// <param name="markup">The markup content.</param>
    /// <param name="interpretAs">The plain text interpretation.</param>
    /// <returns>New annotation.</returns>
    public static MarkupAnnotation CreateMarkup(string markup, string interpretAs)
    {
        return new MarkupAnnotation {
            IsMarkup = true,
            Markup = markup,
            InterpretMarkupAs = interpretAs,
        };
    }

    /// <summary>
    /// Create a segment with plain text.
    /// </summary>
    /// <param name="text">The plain text.</param>
    /// <returns>New annotation.</returns>
    public static MarkupAnnotation CreateText(string text)
    {
        return new MarkupAnnotation {  IsMarkup = false, Text = text };
    }
}

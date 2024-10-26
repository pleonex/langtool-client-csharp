namespace PleOps.LanguageTool.Client.TextCheck;

using System.Collections.ObjectModel;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Represents a text with markup codes.
/// </summary>
public class MarkupText
{
    private static readonly JsonSerializerOptions serializerOptions = new() {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        IgnoreReadOnlyFields = false,
        WriteIndented = false,
    };

    /// <summary>
    /// Gets the collection of annotations that forms the text.
    /// </summary>
    public Collection<MarkupAnnotation> Annotation { get; init; } = [];

    internal string ToRequestBody()
    {
        return JsonSerializer.Serialize(this, serializerOptions);
    }
}

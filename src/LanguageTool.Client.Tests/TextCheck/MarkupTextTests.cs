namespace PleOps.LanguageTool.Client.Tests.TextCheck;

using PleOps.LanguageTool.Client.TextCheck;

[TestFixture]
public class MarkupTextTests
{
    [Test]
    public void PayloadSerializationWithMarkupAndPlain()
    {
        string expected = "{\"annotation\":[" +
            "{\"text\":\"A \"}," +
            "{\"markup\":\"<b>\"}," +
            "{\"text\":\"test\"}," +
            "{\"markup\":\"</b>\"}" +
            "]}";

        var markup = new MarkupText {
            Annotation = [
                MarkupAnnotation.CreateText("A "),
                MarkupAnnotation.CreateMarkup("<b>"),
                MarkupAnnotation.CreateText("test"),
                MarkupAnnotation.CreateMarkup("</b>"),
            ],
        };

        string actual = markup.ToRequestBody();

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void PayloadSerializationWithMarkupInterpretAs()
    {
        string expected = "{\"annotation\":[" +
            "{\"markup\":\"<p>\",\"interpretAs\":\"\\n\\n\"}" +
            "]}";

        var markup = new MarkupText {
            Annotation = [
                MarkupAnnotation.CreateMarkup("<p>", "\n\n"),
            ],
        };

        string actual = markup.ToRequestBody();

        Assert.That(actual, Is.EqualTo(expected));
    }
}

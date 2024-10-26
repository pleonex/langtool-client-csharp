namespace PleOps.LanguageTool.Client.Tests.TextCheck;

using System.Text.RegularExpressions;
using FluentAssertions;
using PleOps.LanguageTool.Client.TextCheck;

[TestFixture]
public class MarkupTextBuilderTests
{
    [Test]
    public void BuildMarkupFromCurlyBracesCodes()
    {
        var regex = new Regex(@"({[\w:]+})");

        string input = "So, this is {color:49}Aurora{color:51}'s new Warlord...?";

        var expected = new MarkupText {
            Annotation = [
                MarkupAnnotation.CreateText("So, this is "),
                MarkupAnnotation.CreateMarkup("{color:49}"),
                MarkupAnnotation.CreateText("Aurora"),
                MarkupAnnotation.CreateMarkup("{color:51}"),
                MarkupAnnotation.CreateText("'s new Warlord...?"),
            ],
        };

        var builder = new RegexMarkupBuilder(regex);
        var actual = builder.Build(input);

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void BuildMarkupFromCurlyBracesCodesBeginningAndEnd()
    {
        var regex = new Regex(@"({[\w:]+})");

        string input = "{0}Testing{1}end{2}";

        var expected = new MarkupText {
            Annotation = [
                MarkupAnnotation.CreateMarkup("{0}"),
                MarkupAnnotation.CreateText("Testing"),
                MarkupAnnotation.CreateMarkup("{1}"),
                MarkupAnnotation.CreateText("end"),
                MarkupAnnotation.CreateMarkup("{2}"),
            ],
        };

        var builder = new RegexMarkupBuilder(regex);
        var actual = builder.Build(input);

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void BuildMarkupFromCurlyBracesCodesConsecutive()
    {
        var regex = new Regex(@"({[\w:]+})");

        string input = "Testing{0}{1}end";

        var expected = new MarkupText {
            Annotation = [
                MarkupAnnotation.CreateText("Testing"),
                MarkupAnnotation.CreateMarkup("{0}"),
                MarkupAnnotation.CreateMarkup("{1}"),
                MarkupAnnotation.CreateText("end"),
            ],
        };

        var builder = new RegexMarkupBuilder(regex);
        var actual = builder.Build(input);

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void BuildMarkupWithMultilineInput()
    {
        var regex = new Regex(@"({[\w:]+})");

        string input = "You're the only one I want to work for, {lord_name}...{wait:48}\n"
            + "Please...{wait:48} Please, let me join you!";

        var expected = new MarkupText {
            Annotation = [
                MarkupAnnotation.CreateText("You're the only one I want to work for, "),
                MarkupAnnotation.CreateMarkup("{lord_name}"),
                MarkupAnnotation.CreateText("..."),
                MarkupAnnotation.CreateMarkup("{wait:48}"),
                MarkupAnnotation.CreateText("\nPlease..."),
                MarkupAnnotation.CreateMarkup("{wait:48}"),
                MarkupAnnotation.CreateText(" Please, let me join you!"),
            ],
        };

        var builder = new RegexMarkupBuilder(regex);
        var actual = builder.Build(input);

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void BuildMarkupWithMultipleCaptureGroups()
    {
        var regex = new Regex(@"({[\w:]+})|(\\p)");

        string input = "You can use {color:50}items{color:51}... " +
            "And you can use {color:50}Warrior Skills{color:51}...\\p";

        var expected = new MarkupText {
            Annotation = [
                MarkupAnnotation.CreateText("You can use "),
                MarkupAnnotation.CreateMarkup("{color:50}"),
                MarkupAnnotation.CreateText("items"),
                MarkupAnnotation.CreateMarkup("{color:51}"),
                MarkupAnnotation.CreateText("... And you can use "),
                MarkupAnnotation.CreateMarkup("{color:50}"),
                MarkupAnnotation.CreateText("Warrior Skills"),
                MarkupAnnotation.CreateMarkup("{color:51}"),
                MarkupAnnotation.CreateText("..."),
                MarkupAnnotation.CreateMarkup("\\p")
            ],
        };

        var builder = new RegexMarkupBuilder(regex);
        var actual = builder.Build(input);

        actual.Should().BeEquivalentTo(expected);
    }
}

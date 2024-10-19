namespace PleOps.LanguageTool.Client;

using PleOps.LanguageTool.Client.Generated.Check;

/// <summary>
/// Information about a supported LanguageTool language.
/// </summary>
/// <param name="Name">A language name like 'French' or 'English (Australia)'.</param>
/// <param name="Code">A language code like 'en'.</param>
/// <param name="LongCode">A language code like 'en-US' or 'ca-ES-valencia'.</param>
public record LanguageInfo(string Name, string Code, string LongCode)
{
    internal LanguageInfo(CheckPostResponse_language language)
        : this(language.Name!, GetShortCode(language.Code!), language.Code!)
    {
    }

    internal LanguageInfo(CheckPostResponse_language_detectedLanguage language)
        : this(language.Name!, GetShortCode(language.Code!), language.Code!)
    {
    }

    private static string GetShortCode(string code)
    {
        int separatorIdx = code.IndexOf('-');
        if (separatorIdx == -1) {
            return code;
        }

        return code[..separatorIdx];
    }
}

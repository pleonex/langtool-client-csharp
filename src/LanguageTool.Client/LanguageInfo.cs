namespace PleOps.LanguageTool.Client;

/// <summary>
/// Information about a supported LanguageTool language.
/// </summary>
/// <param name="Name">A language name like 'French' or 'English (Australia)'.</param>
/// <param name="Code">A language code like 'en'.</param>
/// <param name="LongCode">A language code like 'en-US' or 'ca-ES-valencia'.</param>
public record LanguageInfo(string Name, string Code, string LongCode);

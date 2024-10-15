namespace PleOps.LanguageTool.Client;

using PleOps.LanguageTool.Client.Check;

/// <summary>
/// Parameters to run the text checks.
/// </summary>
public record CheckParameters
{
    /// <summary>
    /// Gets or sets a language code like `en-US`, `de-DE`, `fr`, or `auto` to guess the language automatically.
    /// </summary>
    /// <remarks>
    /// For languages with variants (English, German, Portuguese) spell checking
    /// will only be activated when you specify the variant, e.g. `en-GB` instead of just `en`.
    /// </remarks>
    public required string Language { get; init; }

    /// <summary>
    /// Gets or sets a language code of the user's native language,
    /// enabling false friends checks for some language pairs.
    /// </summary>
    public string MotherTongue { get; init; } = "";

    /// <summary>
    /// Gets or sets a list of preferred language variants.
    /// </summary>
    /// <remarks>
    /// The language detector used with `auto` can detect e.g. English,
    /// but it cannot decide whether British English or American English is used.
    /// Thus this parameter can be used to specify the preferred variants like `en-GB` and `de-AT`.
    /// Only available with `language=auto`. You should set variants for at least German and English,
    /// as otherwise the spell checking will not work for those.
    /// </remarks>
    public string[] PreferredVariants { get; init; } = [];

    /// <summary>
    /// Gets or sets the IDs of rules to be enabled.
    /// </summary>
    /// <remarks>
    /// Note that 'level' still applies,
    /// so the rule won't run unless 'level' is set to a level that activates the rule.
    /// </remarks>
    public string[] EnabledRules { get; init; } = [];

    /// <summary>
    /// Gets or sets the IDs of rules to be disabled.
    /// </summary>
    public string[] DisabledRules { get; init; } = [];

    /// <summary>
    /// Gets or sets the IDs of categories to be enabled.
    /// </summary>
    public string[] EnabledCategories { get; init; } = [];

    /// <summary>
    /// Gets or sets the IDs of categories to be disabled.
    /// </summary>
    public string[] DisabledCategories { get; init; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether only the rules and categories
    /// whose IDs are specified with enabled rules or enabled categories are enabled.
    /// </summary>
    public bool EnabledOnly { get; init; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether additional rules will be activated,
    /// i.e. rules that you might only find useful when checking formal text.
    /// </summary>
    public bool Picky { get; init; }

    internal CheckPostRequestBody ToRequestBody()
    {
        return new CheckPostRequestBody {
            Language = Language,
            MotherTongue = MotherTongue,
            PreferredVariants = string.Join(',', PreferredVariants),
            EnabledRules = string.Join(',', EnabledRules),
            DisabledRules = string.Join(',', DisabledRules),
            EnabledCategories = string.Join(',', EnabledCategories),
            DisabledCategories = string.Join(',', DisabledCategories),
            EnabledOnly = EnabledOnly,
            Level = Picky ? CheckPostRequestBody_level.Picky : CheckPostRequestBody_level.Default,
        };
    }
}

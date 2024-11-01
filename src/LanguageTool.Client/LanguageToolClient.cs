﻿namespace PleOps.LanguageTool.Client;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using PleOps.LanguageTool.Client.Generated;
using PleOps.LanguageTool.Client.Generated.Check;
using PleOps.LanguageTool.Client.Generated.Languages;
using PleOps.LanguageTool.Client.TextCheck;

/// <summary>
/// LanguageTool REST API client.
/// </summary>
/// <remarks>
/// This a wrapper over the generated REST client from its OpenAPI specification.
/// It does not support Premium features from the cloud version.
/// </remarks>
public class LanguageToolClient
{
    private readonly GeneratedLanguageToolClient client;
    private readonly ConcurrentDictionary<string, int> userDictionary;

    internal LanguageToolClient(GeneratedLanguageToolClient client)
    {
        ArgumentNullException.ThrowIfNull(client);
        this.client = client;

        // Use dictionary because it's the only type to support fast concurrent remove.
        userDictionary = new ConcurrentDictionary<string, int>();
    }

    /// <summary>
    /// Gets or sets a value indicating whether to the user's dictionary is case sensitive for ignoring matching words.
    /// </summary>
    public bool UserDictionaryCaseSensitive { get; set; }

    /// <summary>
    /// Gets the current in-memory user dictionary.
    /// </summary>
    public IEnumerable<string> UserDictionary => userDictionary.Keys;

    /// <summary>
    /// Add a word to the current in-memory dictionary.
    /// </summary>
    /// <param name="words">The words to be added. Must not be a phrase, i.e. cannot contain white space.</param>
    /// <remarks>The word is added to a global dictionary that applies to all languages.</remarks>
    public void AddUserWords(params string[] words)
    {
        foreach (string word in words) {
            userDictionary[word] = 0;
        }
    }

    /// <summary>
    /// Remove word from the current in-memory dictionary.
    /// </summary>
    /// <param name="words">The words to be removed.</param>
    public void RemoveUserWords(params string[] words) {
        foreach (string word in words) {
            _ = userDictionary.TryRemove(word, out int _);
        }
    }

    /// <summary>
    /// Add the lines from the text file in the current in-memory dictionary.
    /// </summary>
    /// <param name="dictionaryPath">Path to the text file with the user words.</param>
    /// <remarks>
    /// The format is one word per line, case sensitive.
    /// It trims starting and ending spaces. Lines starting with '#' are ignored.
    /// </remarks>
    public void AddUserDictionary(string dictionaryPath)
    {
        IEnumerable<string> words = File.ReadAllLines(dictionaryPath)
            .Select(l => l.Trim())
            .Where(l => l.Length > 0 && !l.StartsWith('#'));

        foreach (string word in words) {
            userDictionary[word] = 0;
        }
    }

    /// <summary>
    /// Check a text with LanguageTool for possible style and grammar issues ignoring results matching user words.
    /// </summary>
    /// <param name="text">The text to be checked (without markup).</param>
    /// <param name="language">
    /// A language code like `en-US`, `de-DE`, `fr`, or `auto` to guess the language automatically.
    /// For languages with variants (English, German, Portuguese) spell checking
    /// will only be activated when you specify the variant, e.g. `en-GB` instead of just `en`.
    /// </param>
    /// <param name="picky">
    /// If set, additional rules will be activated,
    /// i.e. rules that you might only find useful when checking formal text.
    /// </param>
    /// <returns>A list of detected issues.</returns>
    public async Task<TextCheckResult> CheckPlainTextAsync(
        string text,
        string language,
        bool picky)
    {
        var parameters = new TextCheckParameters {
            Language = language,
            Picky = picky,
        };
        return await CheckPlainTextAsync(text, parameters);
    }

    /// <summary>
    /// Check a text with LanguageTool for possible style and grammar issues ignoring results matching user words.
    /// </summary>
    /// <param name="text">The text to be checked (without markup).</param>
    /// <param name="parameters">The parameters to run the check.</param>
    /// <returns>A list of detected issues.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<TextCheckResult> CheckPlainTextAsync(string text, TextCheckParameters parameters)
    {
        CheckPostRequestBody body = parameters.ToRequestBody();
        body.Text = text;

        CheckPostResponse response = await client.Check.PostAsync(body)
            ?? throw new InvalidOperationException("Invalid response data");

        // Ignore matches due to words in the user dictionary.
        IEnumerable<CheckPostResponse_matches> matches = FilterMatchesFromDictionary(response);

        return new TextCheckResult(response, matches);
    }

    /// <summary>
    /// Check a markup text with LanguageTool for possible style and grammar issues
    /// ignoring results matching user words.
    /// </summary>
    /// <param name="jsonData">
    /// The text to be checked, given as a JSON document that specifies what's text and what's markup.
    /// Markup will be ignored when looking for errors.
    /// See <see href="https://languagetool.org/http-api/" /> for the JSON format.
    /// </param>
    /// <param name="parameters">The parameters to run the check.</param>
    /// <returns>A list of detected issues.</returns>
    public async Task<TextCheckResult> CheckMarkupTextAsync(string jsonData, TextCheckParameters parameters)
    {
        CheckPostRequestBody body = parameters.ToRequestBody();
        body.Data = jsonData;

        CheckPostResponse response = await client.Check.PostAsync(body)
            ?? throw new InvalidOperationException("Invalid response data");

        // Ignore matches due to words in the user dictionary.
        IEnumerable<CheckPostResponse_matches> matches = FilterMatchesFromDictionary(response);

        return new TextCheckResult(response, matches);
    }

    /// <summary>
    /// Check a markup text with LanguageTool for possible style and grammar issues
    /// ignoring results matching user words.
    /// </summary>
    /// <param name="text">
    /// The text to be checked, given as a document that specifies what's text and what's markup.
    /// Markup will be ignored when looking for errors.
    /// </param>
    /// <param name="parameters">The parameters to run the check.</param>
    /// <returns>A list of detected issues.</returns>
    public async Task<TextCheckResult> CheckMarkupTextAsync(MarkupText text, TextCheckParameters parameters)
    {
        return await CheckMarkupTextAsync(text.ToRequestBody(), parameters);
    }

    /// <summary>
    /// Get a list of supported languages.
    /// </summary>
    /// <returns>An array of language objects.</returns>
    public async Task<IEnumerable<LanguageInfo>> GetSupportedLanguagesAsync()
    {
        List<Languages> response = await client.Languages.GetAsync()
            ?? throw new InvalidOperationException("Invalid response data");

        return response.Select(l => new LanguageInfo(l.Name!, l.Code!, l.LongCode!));
    }

    private IEnumerable<CheckPostResponse_matches> FilterMatchesFromDictionary(CheckPostResponse response)
    {
        var textCulture = new CultureInfo(response.Language!.Code!);
        var comparer = StringComparer.Create(textCulture, ignoreCase: !UserDictionaryCaseSensitive);
        IEnumerable<CheckPostResponse_matches> matches = response.Matches!
            .Where(m => {
                var context = m.Context!;
                string matchContent = context.Text!.Substring(context.Offset!.Value, context.Length!.Value);
                return !userDictionary.Keys.Contains(matchContent, comparer);
            });

        return matches;
    }
}

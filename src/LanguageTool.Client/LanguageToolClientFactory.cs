namespace PleOps.LanguageTool.Client;

using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

/// <summary>
/// Static factory of LanguageTool clients.
/// </summary>
public static class LanguageToolClientFactory
{
    /// <summary>
    /// Create a new LanguageTool client with the default options.
    /// </summary>
    /// <param name="baseAddress">The address of the server.</param>
    /// <returns>LanguageTool client.</returns>
    public static LanguageToolClient Create(string baseAddress)
    {
        var options = new LanguageToolClientOptions();
        options.BaseAddress = baseAddress;

        return Create(options);
    }

    /// <summary>
    /// Create a new LanguageTool client.
    /// </summary>
    /// <param name="options">The client's options.</param>
    /// <returns>LanguageTool client.</returns>
    public static LanguageToolClient Create(LanguageToolClientOptions options)
    {
        IRequestOption[] httpOptions = [ options.RetryOptions ];
        HttpClient httpClient = KiotaClientFactory.Create(null, httpOptions);
        httpClient.BaseAddress = new Uri(options.BaseAddress);

        var authProvider = new AnonymousAuthenticationProvider();
        var adapter = new HttpClientRequestAdapter(authProvider, httpClient: httpClient);

        var internalClient = new InternalLanguageToolClient(adapter);
        return new LanguageToolClient(internalClient);
    }
}

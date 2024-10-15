namespace PleOps.LanguageTool.Client;

using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

public static class LanguageToolClientFactory
{
    public static LanguageToolClient Create(string hostname)
    {
        IRequestOption[] httpOptions = [];
        HttpClient httpClient = KiotaClientFactory.Create(null, httpOptions);
        httpClient.BaseAddress = new Uri(hostname);

        var authProvider = new AnonymousAuthenticationProvider();
        var adapter = new HttpClientRequestAdapter(authProvider, httpClient: httpClient);

        var internalClient = new InternalLanguageToolClient(adapter);
        return new LanguageToolClient(internalClient);
    }
}

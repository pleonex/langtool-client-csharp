namespace PleOps.LanguageTool.Client;

using System.Net;
using Microsoft.Kiota.Http.HttpClientLibrary.Middleware.Options;

/// <summary>
/// Options for the LanguageTool client.
/// </summary>
public class LanguageToolClientOptions
{
    /// <summary>
    /// Gets or sets the base address of the LanguageTool server.
    /// </summary>
    public string BaseAddress { get; set; } = "http://localhost:8010/v2/";

    /// <summary>
    /// Gets or sets the options for the retry handler of the client.
    /// </summary>
    public RetryHandlerOption RetryOptions { get; set; } = new() {
        ShouldRetry = (_, _, r) =>
            r.StatusCode is >= HttpStatusCode.InternalServerError or HttpStatusCode.RequestTimeout,
    };
}

namespace PleOps.LanguageTool.Client.TextCheck;

using System.Globalization;
using PleOps.LanguageTool.Client.Generated.Check;

/// <summary>
/// Information about the LanguageTool software.
/// </summary>
public record LanguageToolSoftwareInfo
{
    internal LanguageToolSoftwareInfo(CheckPostResponse_software response)
    {
        ApiVersion = response.ApiVersion!.Value;
        BuildDate = DateOnly.ParseExact(response.BuildDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        Name = response.Name!;
        Premium = response.Premium!.Value;
        Status = response.Status!;
        Version = Version.Parse(response.Version!);
    }

    /// <summary>
    /// Gets the version of this API response.
    /// </summary>
    public int ApiVersion { get; }

    /// <summary>
    /// Gets the date when the software was built.
    /// </summary>
    public DateOnly BuildDate { get; }

    /// <summary>
    /// Gets the name of the software.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets a value indicating whether the request was done with premium features enabled.
    /// </summary>
    public bool Premium { get; }

    /// <summary>
    /// Gets an optional warning, e.g., when the API format is not stable.
    /// </summary>
    public string Status { get; }

    /// <summary>
    /// Gets the software version.
    /// </summary>
    public Version Version { get; }
}

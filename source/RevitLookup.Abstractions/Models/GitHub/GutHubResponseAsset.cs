using System.Text.Json.Serialization;

namespace RevitLookup.Abstractions.Models.GitHub;

[Serializable]
public sealed class GutHubResponseAsset
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("browser_download_url")] public string? DownloadUrl { get; set; }
}
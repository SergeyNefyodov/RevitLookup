namespace RevitLookup.Abstractions.Options;

public sealed class FoldersOptions
{
    public required string RootFolder { get; set; }
    public required string ConfigFolder { get; set; }
    public required string DownloadsFolder { get; set; }
    public required string GeneralSettingsPath { get; set; }
    public required string RenderSettingsPath { get; set; }
}
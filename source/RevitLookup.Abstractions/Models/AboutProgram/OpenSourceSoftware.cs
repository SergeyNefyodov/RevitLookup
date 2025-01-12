namespace RevitLookup.Abstractions.Models.AboutProgram;

public sealed class OpenSourceSoftware
{
    public required string SoftwareName { get; set; }
    public required string SoftwareUri { get; set; }
    public required string LicenseName { get; set; }
    public required string LicenseUri { get; set; }
}
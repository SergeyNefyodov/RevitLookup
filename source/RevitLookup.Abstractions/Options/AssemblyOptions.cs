namespace RevitLookup.Abstractions.Options;

public sealed class AssemblyOptions
{
    public required string Framework { get; set; }
    public required Version Version { get; set; }
    public required bool HasAdminAccess { get; set; }
}
namespace RevitLookup.Abstractions.Models.EventArgs;

public sealed class EventInfoArgs
{
    public required string EventName { get; set; }
    public required object Arguments { get; set; }
}
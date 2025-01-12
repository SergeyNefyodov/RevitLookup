namespace RevitLookup.Abstractions.Models.UserInterface;

public sealed class NavigationCardGroup
{
    public required string GroupName { get; set; }
    public required List<NavigationCardItem> Items { get; set; }
}
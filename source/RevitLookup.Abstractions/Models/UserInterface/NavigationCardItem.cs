using System.Windows.Input;
using Wpf.Ui.Controls;

namespace RevitLookup.Abstractions.Models.UserInterface;

public sealed class NavigationCardItem
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required SymbolRegular Icon { get; set; }
    public required ICommand Command { get; set; }
    public object? CommandParameter { get; set; }
}
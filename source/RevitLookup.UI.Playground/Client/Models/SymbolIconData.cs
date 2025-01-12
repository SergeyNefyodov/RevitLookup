using Wpf.Ui.Controls;

namespace RevitLookup.UI.Playground.Client.Models;

/// <summary>
/// IconData class for icons in icon page
/// </summary>
public sealed class SymbolIconData
{
    public required string Name { get; init; }
    public required SymbolRegular Icon { get; init; }
    public required string Code { get; init; }
    public string TextGlyph => $"&#x{Code};";
}
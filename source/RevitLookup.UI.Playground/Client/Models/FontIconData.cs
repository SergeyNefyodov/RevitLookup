namespace RevitLookup.UI.Playground.Client.Models;

/// <summary>
/// IconData class for icons in icon page
/// </summary>
[Serializable]
public class FontIconData
{
    public required string Name { get; set; }
    public required string Code { get; set; }

    public string Character => char.ConvertFromUtf32(Convert.ToInt32(Code, 16));
    public string CodeGlyph => "\\x" + Code;
    public string TextGlyph => "&#x" + Code + ";";
}
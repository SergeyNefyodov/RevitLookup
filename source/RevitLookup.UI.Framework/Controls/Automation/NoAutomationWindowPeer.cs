using System.Windows;
using System.Windows.Automation.Peers;

namespace RevitLookup.UI.Framework.Controls.Automation;

/// <summary>
///     Windows peer disabling automation. Removes freezes when using Tooltip, Popup
/// </summary>
/// <remarks>
///     https://github.com/dotnet/wpf/issues/5807
/// </remarks>
public sealed class NoAutomationWindowPeer(Window owner) : WindowAutomationPeer(owner)
{
    protected override List<AutomationPeer> GetChildrenCore()
    {
        return [];
    }
}
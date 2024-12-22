using System.Reflection;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

namespace RevitLookup.Utils;

/// <summary>
///     Contains extension methods for creating and managing custom Ribbon elements in the Revit UI.
///     These extensions provide simplified methods for adding panels, buttons, and other controls 
///     to the "Add-ins" tab or any custom tab in the Revit ribbon.
///     These utilities streamline the process of integrating external commands and tools 
///     into the Revit user interface.
/// </summary>
public static class RibbonExtensions
{
    /// <summary>
    ///     Creates or retrieves an existing panel in a specified tab of the Revit ribbon.
    /// </summary>
    /// <param name="application">The Revit application instance.</param>
    /// <param name="panelName">The name of the panel to create.</param>
    /// <param name="tabName">The name of the tab in which the panel should be located.</param>
    /// <returns>The created or existing Ribbon panel.</returns>
    /// <remarks>
    ///     If the tab doesn't exist, it will be created first. 
    ///     If a panel with the specified name already exists within the tab, it will return that panel; otherwise, a new one will be created.
    /// </remarks>
    /// <exception cref="Autodesk.Revit.Exceptions.ArgumentException">Thrown when panelName or tabName is empty.</exception>
    /// <exception cref="Autodesk.Revit.Exceptions.InvalidOperationException">
    ///     Thrown if more than 100 panels were created, or if the maximum number of custom tabs (20) has been exceeded.
    /// </exception>
    public static RibbonPanel CreatePanel(this UIControlledApplication application, string panelName, string tabName)
    {
        foreach (var tab in ComponentManager.Ribbon.Tabs)
        {
            if ((tab.Title == tabName || tab.Id == tabName) && tab.IsVisible)
            {
                var cachedTabs = GetCachedTabs();
                if (cachedTabs.TryGetValue(tab.Id, out var cachedPanels))
                {
                    if (cachedPanels.TryGetValue(panelName, out var cachedPanel))
                    {
                        return cachedPanel;
                    }
                }

                var (internalPanel, panel) = CreateInternalPanel(tab.Id, panelName);
                tab.Panels.Add(internalPanel);
                return panel;
            }
        }

        application.CreateRibbonTab(tabName);
        return application.CreateRibbonPanel(tabName, panelName);
    }

    /// <summary>
    ///     Retrieves the internal <see cref="Autodesk.Windows.RibbonPanel"/> instance associated with the specified <see cref="RibbonPanel"/>.
    ///     This method uses reflection to access the private field "m_RibbonPanel" within the provided <see cref="RibbonPanel"/>.
    /// </summary>
    /// <param name="panel">The <see cref="RibbonPanel"/> to extract the internal <see cref="Autodesk.Windows.RibbonPanel"/> from.</param>
    /// <returns>The internal <see cref="Autodesk.Windows.RibbonPanel"/> instance.</returns>
    public static Autodesk.Windows.RibbonPanel GetInternalPanel(this RibbonPanel panel)
    {
        var panelField = panel.GetType().GetField("m_RibbonPanel", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;
        return (Autodesk.Windows.RibbonPanel) panelField.GetValue(panel)!;
    }

    [Pure]
    public static Dictionary<string, Dictionary<string, RibbonPanel>> GetCachedTabs()
    {
        var applicationType = typeof(UIApplication);
        var panelsField = applicationType.GetField("m_ItemsNameDictionary", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly)!;
        return (Dictionary<string, Dictionary<string, RibbonPanel>>) panelsField.GetValue(null)!;
    }

    private static RibbonPanel CreatePanel(Autodesk.Windows.RibbonPanel panel, string tabId)
    {
        var type = typeof(RibbonPanel);
#if NETCOREAPP
        var constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly,
            [typeof(Autodesk.Windows.RibbonPanel), typeof(string)])!;
#else
        var constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly,
            null,
            [typeof(Autodesk.Windows.RibbonPanel), typeof(string)],
            null)!;
#endif
        return (RibbonPanel) constructorInfo.Invoke([panel, tabId]);
    }

    private static (Autodesk.Windows.RibbonPanel internalPanel, RibbonPanel panel) CreateInternalPanel(string tabId, string panelName)
    {
        var internalPanel = new Autodesk.Windows.RibbonPanel
        {
            Source = new RibbonPanelSource
            {
                Title = panelName
            }
        };

        var cachedTabs = GetCachedTabs();
        if (!cachedTabs.TryGetValue(tabId, out var cachedPanels))
        {
            cachedTabs[tabId] = cachedPanels = new Dictionary<string, RibbonPanel>();
        }

        var panel = CreatePanel(internalPanel, tabId);
        panel.Name = panelName;
        cachedPanels[panelName] = panel;

        return (internalPanel, panel);
    }
}
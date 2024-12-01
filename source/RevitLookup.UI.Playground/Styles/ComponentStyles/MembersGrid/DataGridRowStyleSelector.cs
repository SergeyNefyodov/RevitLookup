using System.Collections;
using System.Windows;
using System.Windows.Controls;
using LookupEngine.Abstractions.Configuration;
using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.UI.Playground.Styles.ComponentStyles.MembersGrid;

/// <summary>
///     Data grid row style selector
/// </summary>
public sealed class DataGridRowStyleSelector : StyleSelector
{
    public override Style? SelectStyle(object item, DependencyObject container)
    {
        var member = (ObservableDecomposedMember)item;
        var presenter = (FrameworkElement)container;

        var styleName = member.Value.RawValue switch
                        {
                            Exception => "ExceptionDataGridRowStyle",
                            ICollection { Count: > 0 } => "HandledDataGridRowStyle",
                            _ => null
                        }
                        ??
                        member.Value.Descriptor switch
                        {
                            IDescriptorEnumerator { IsEmpty: false } => "HandleDataGridRowStyle",
                            IDescriptorCollector => "HandledDataGridRowStyle",
                            _ => "DefaultLookupDataGridRowStyle"
                        };

        return (Style)presenter.FindResource(styleName);
    }
}
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using LookupEngine.Abstractions.ComponentModel;
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

        var styleName = SelectByType(member.Value.RawValue) ??
                        SelectByDescriptor(member.Value.Descriptor);

        return (Style)presenter.FindResource(styleName);
    }

    private static string? SelectByType(object? value)
    {
        return value switch
        {
            Exception => "ExceptionDataGridRowStyle",
            ICollection { Count: > 0 } => "HandledDataGridRowStyle",
            _ => null
        };
    }

    private static string SelectByDescriptor(Descriptor? descriptor)
    {
        return descriptor switch
        {
            IDescriptorEnumerator { IsEmpty: false } => "HandleDataGridRowStyle",
            IDescriptorCollector => "HandledDataGridRowStyle",
            _ => "DefaultLookupDataGridRowStyle"
        };
    }
}
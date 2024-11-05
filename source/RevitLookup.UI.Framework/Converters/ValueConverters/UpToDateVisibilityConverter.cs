using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using RevitLookup.Abstractions.States;

namespace RevitLookup.UI.Framework.Converters.ValueConverters;

public sealed class UpToDateVisibilityConverter : MarkupExtension, IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var state = (SoftwareUpdateState) values[0];
        var isUpdating = (bool) values[1];
        var isUpdateChecked = (bool) values[2];
        return state == SoftwareUpdateState.UpToDate && !isUpdating && isUpdateChecked ? Visibility.Visible : Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}
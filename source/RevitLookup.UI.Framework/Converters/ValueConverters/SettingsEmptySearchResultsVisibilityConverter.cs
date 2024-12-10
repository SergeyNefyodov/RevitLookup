using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace RevitLookup.UI.Framework.Converters.ValueConverters;

public sealed class SettingsEmptySearchResultsVisibilityConverter : MarkupExtension, IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length != 3) throw new ArgumentException("Invalid parameters");

        var items = (ICollection)values[0]!;
        if (items.Count > 0) return Visibility.Collapsed;

        if (values[1] is > 0) return Visibility.Collapsed;
        if (values[2] is false) return Visibility.Collapsed;

        return Visibility.Visible;
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
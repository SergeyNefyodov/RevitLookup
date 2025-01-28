using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace RevitLookup.UI.Framework.Converters.ValueConverters;

public sealed class SettingsEntriesEmptyAfterInitializationConverter : MarkupExtension, IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is not int collectionSize) return Visibility.Collapsed;
        if (values[1] is not bool isInitialized) return Visibility.Collapsed;

        if (!isInitialized) return Visibility.Collapsed;
        if (collectionSize > 0) return Visibility.Collapsed;

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
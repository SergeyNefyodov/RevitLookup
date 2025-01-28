using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace RevitLookup.UI.Playground.Client.Helpers;

/// <summary>
/// Converts a null value to Visibility.Collapsed
/// </summary>
internal sealed class NullToVisibilityConverter : MarkupExtension, IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is null ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}
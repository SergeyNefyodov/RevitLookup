using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace RevitLookup.UI.Framework.Converters.ValueConverters;

public sealed class WidthToUniformColumnsConverter : MarkupExtension, IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var width = (double) value!;
        var columns = (int) Math.Floor(width / 400d);
        return columns > 0 ? columns : 1;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}
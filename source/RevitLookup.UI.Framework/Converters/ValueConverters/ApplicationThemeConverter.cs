using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Wpf.Ui.Appearance;

namespace RevitLookup.UI.Framework.Converters.ValueConverters;

public sealed class ApplicationThemeConverter : MarkupExtension, IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var applicationTheme = (ApplicationTheme) value!;
        return applicationTheme switch
        {
            ApplicationTheme.Auto => "Auto",
            ApplicationTheme.Light => "Light",
            ApplicationTheme.Dark => "Dark",
            ApplicationTheme.HighContrast => "High contrast",
            ApplicationTheme.Unknown => throw new NotSupportedException(),
            _ => throw new ArgumentOutOfRangeException()
        };
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
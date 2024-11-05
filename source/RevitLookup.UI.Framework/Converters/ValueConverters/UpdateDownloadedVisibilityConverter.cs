using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using RevitLookup.Abstractions.States;

namespace RevitLookup.UI.Framework.Converters.ValueConverters;

public sealed class UpdateDownloadedVisibilityConverter : MarkupExtension, IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var state = (SoftwareUpdateState) value!;
        return state is SoftwareUpdateState.ReadyToInstall ? Visibility.Visible : Visibility.Collapsed;
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
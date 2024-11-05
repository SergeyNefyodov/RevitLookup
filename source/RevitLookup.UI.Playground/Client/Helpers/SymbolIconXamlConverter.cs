using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using Wpf.Ui.Controls;

namespace RevitLookup.UI.Playground.Client.Helpers;

public sealed class SymbolIconXamlConverter : MarkupExtension, IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is string text) return text;

        var icon = (SymbolRegular)values[0];
        var filled = (bool)values[1];

        var builder = new StringBuilder();
        builder.Append("<ui:SymbolIcon Symbol=\"");
        builder.Append(icon);
        builder.Append('"');
        if (filled)
        {
            builder.Append(" Filled=\"");
            builder.Append(filled);
            builder.Append('"');
        }

        builder.Append(" />");

        return builder.ToString();
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
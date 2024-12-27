using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.UI.Framework.Converters.ValueConverters;

public sealed class SingleDescriptorLabelConverter : DescriptorLabelConverter
{
    public override object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var member = (ObservableDecomposedObject) value!;
        if (!TryConvertInvalidNames(member.Name, out var name))
        {
            name = CreateSingleName(member.Name, member.Description);
        }

        return name;
    }

    private static string CreateSingleName(string name, string? description)
    {
        return string.IsNullOrEmpty(description) ? name : description!;
    }
}

public sealed class CombinedDescriptorLabelConverter : DescriptorLabelConverter
{
    public override object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var member = (ObservableDecomposedMember) value!;
        if (!TryConvertInvalidNames(member.Value.Name, out var name))
        {
            name = CreateCombinedName(member.Value.Name, member.Value.Description);
        }

        return name;
    }

    private static string CreateCombinedName(string name, string? description)
    {
        if (string.IsNullOrEmpty(description)) return name;
        if (description!.EndsWith(name, StringComparison.OrdinalIgnoreCase)) return description;

        return $"{description}: {name}";
    }
}

public abstract class DescriptorLabelConverter : MarkupExtension, IValueConverter
{
    protected bool TryConvertInvalidNames(string text, out string result)
    {
        result = text switch
        {
            null => "<null>",
            "" => "<empty>",
            _ => text
        };

        return text != result;
    }

    public abstract object Convert(object? value, Type targetType, object? parameter, CultureInfo culture);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}
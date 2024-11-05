using System.Windows;
using System.Windows.Media;

namespace RevitLookup.UI.Framework.Utils;

public static class VisualExtensions
{
    public static T? FindVisualParent<T>(this FrameworkElement element) where T : FrameworkElement
    {
        var parentElement = (FrameworkElement?)VisualTreeHelper.GetParent(element);
        while (parentElement != null)
        {
            if (parentElement is T parent)
                return parent;

            parentElement = (FrameworkElement?)VisualTreeHelper.GetParent(parentElement);
        }

        return null;
    }

    public static T? FindVisualParent<T>(this FrameworkElement element, string name) where T : FrameworkElement
    {
        var parentElement = (FrameworkElement?)VisualTreeHelper.GetParent(element);
        while (parentElement != null)
        {
            if (parentElement is T parent)
                if (parentElement.Name == name)
                    return parent;

            parentElement = (FrameworkElement?)VisualTreeHelper.GetParent(parentElement);
        }

        return null;
    }

    public static T? FindVisualChild<T>(this FrameworkElement element) where T : Visual
    {
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
        {
            var childElement = (FrameworkElement?)VisualTreeHelper.GetChild(element, i);
            if (childElement is null) return null;

            if (childElement is T child)
                return child;

            var descendent = FindVisualChild<T>(childElement);
            if (descendent != null) return descendent;
        }

        return null;
    }

    public static T? FindVisualChild<T>(this FrameworkElement element, string name) where T : Visual
    {
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
        {
            var childElement = (FrameworkElement?)VisualTreeHelper.GetChild(element, i);
            if (childElement is null) return null;

            if (childElement is T child)
                if (childElement.Name == name)
                    return child;

            var descendent = FindVisualChild<T>(childElement, name);
            if (descendent != null) return descendent;
        }

        return null;
    }
}
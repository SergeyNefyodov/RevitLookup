using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace RevitLookup.UI.Playground.Client.Controls;

/// <summary>
/// A control that displays an example of a control
/// </summary>
[ContentProperty(nameof(ExampleContent))]
public sealed class ControlExample : Control
{
    static ControlExample()
    {
        CommandManager.RegisterClassCommandBinding(typeof(ControlExample), new CommandBinding(ApplicationCommands.Copy, Copy_SourceCode));
    }

    public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register(
        nameof(HeaderText),
        typeof(string),
        typeof(ControlExample),
        new PropertyMetadata(null)
    );

    public static readonly DependencyProperty ExampleContentProperty = DependencyProperty.Register(
        nameof(ExampleContent),
        typeof(object),
        typeof(ControlExample),
        new PropertyMetadata(null)
    );

    public static readonly DependencyProperty XamlCodeProperty = DependencyProperty.Register(
        nameof(XamlCode),
        typeof(string),
        typeof(ControlExample),
        new PropertyMetadata(null)
    );

    public static readonly DependencyProperty XamlCodeSourceProperty = DependencyProperty.Register(
        nameof(XamlCodeSource),
        typeof(Uri),
        typeof(ControlExample),
        new PropertyMetadata(
            null,
            static (o, args) => ((ControlExample)o).OnXamlCodeSourceChanged((Uri)args.NewValue)
        )
    );

    public static readonly DependencyProperty CsharpCodeProperty = DependencyProperty.Register(
        nameof(CsharpCode),
        typeof(string),
        typeof(ControlExample),
        new PropertyMetadata(null)
    );

    public static readonly DependencyProperty CsharpCodeSourceProperty = DependencyProperty.Register(
        nameof(CsharpCodeSource),
        typeof(Uri),
        typeof(ControlExample),
        new PropertyMetadata(
            null,
            static (o, args) => ((ControlExample)o).OnCsharpCodeSourceChanged((Uri)args.NewValue)
        )
    );

    public string? HeaderText
    {
        get => (string)GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }

    public object? ExampleContent
    {
        get => GetValue(ExampleContentProperty);
        set => SetValue(ExampleContentProperty, value);
    }

    public string? XamlCode
    {
        get => (string)GetValue(XamlCodeProperty);
        set => SetValue(XamlCodeProperty, value);
    }

    public Uri? XamlCodeSource
    {
        get => (Uri)GetValue(XamlCodeSourceProperty);
        set => SetValue(XamlCodeSourceProperty, value);
    }

    public string? CsharpCode
    {
        get => (string)GetValue(CsharpCodeProperty);
        set => SetValue(CsharpCodeProperty, value);
    }

    public Uri? CsharpCodeSource
    {
        get => (Uri)GetValue(CsharpCodeSourceProperty);
        set => SetValue(CsharpCodeSourceProperty, value);
    }

    private void OnXamlCodeSourceChanged(Uri uri)
    {
        XamlCode = LoadResource(uri);
    }

    private void OnCsharpCodeSourceChanged(Uri uri)
    {
        CsharpCode = LoadResource(uri);
    }

    private static void Copy_SourceCode(object sender, RoutedEventArgs e)
    {
        var controlExample = (ControlExample)sender;

        try
        {
            switch (((ExecutedRoutedEventArgs)e).Parameter.ToString())
            {
                case "Copy_XamlCode":
                    if (string.IsNullOrEmpty(controlExample.XamlCode)) break;

                    Clipboard.SetText(controlExample.XamlCode);
                    var peer = UIElementAutomationPeer.CreatePeerForElement((Button)e.OriginalSource);
                    peer.RaiseNotificationEvent(
                        AutomationNotificationKind.Other,
                        AutomationNotificationProcessing.ImportantMostRecent,
                        "Source Code Copied",
                        "ButtonClickedActivity"
                    );

                    break;
                case "Copy_CsharpCode":
                    if (string.IsNullOrEmpty(controlExample.CsharpCode)) break;

                    Clipboard.SetText(controlExample.CsharpCode);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
        catch
        {
            // ignored
        }
    }

    private static string LoadResource(Uri uri)
    {
        try
        {
            if (Application.GetResourceStream(uri) is not { } steamInfo)
            {
                return string.Empty;
            }

            using StreamReader streamReader = new(steamInfo.Stream, Encoding.UTF8);
            return streamReader.ReadToEnd();
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
            return exception.ToString();
        }
    }
}
using System.Windows.Media;
using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using JetBrains.Annotations;
using RevitLookup.Abstractions.ViewModels.Visualization;

namespace RevitLookup.UI.Playground.ViewModels.Visualization;

[UsedImplicitly]
public sealed partial class XyzVisualizationViewModel : ObservableObject, IXyzVisualizationViewModel
{
    [ObservableProperty] private double _axisLength;
    [ObservableProperty] private double _transparency;

    [ObservableProperty] private Color _xColor;
    [ObservableProperty] private Color _yColor;
    [ObservableProperty] private Color _zColor;

    [ObservableProperty] private bool _showPlane;
    [ObservableProperty] private bool _showXAxis;
    [ObservableProperty] private bool _showYAxis;
    [ObservableProperty] private bool _showZAxis;

    public XyzVisualizationViewModel()
    {
        var faker = new Faker();

        MinAxisLength = 0;
        Transparency = faker.Random.Double(0, 100);
        AxisLength = faker.Random.Double(0, 24);
        XColor = Color.FromRgb(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte());
        YColor = Color.FromRgb(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte());
        ZColor = Color.FromRgb(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte());

        ShowPlane = faker.Random.Bool();
        ShowXAxis = faker.Random.Bool();
        ShowYAxis = faker.Random.Bool();
        ShowZAxis = faker.Random.Bool();
    }

    public double MinAxisLength { get; }

    public void RegisterServer(object xyz)
    {
    }

    public void UnregisterServer()
    {
    }
}
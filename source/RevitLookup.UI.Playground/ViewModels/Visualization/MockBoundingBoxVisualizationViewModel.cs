using System.Windows.Media;
using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using JetBrains.Annotations;
using RevitLookup.Abstractions.ViewModels.Visualization;

namespace RevitLookup.UI.Playground.ViewModels.Visualization;

[UsedImplicitly]
public sealed partial class MockBoundingBoxVisualizationViewModel : ObservableObject, IBoundingBoxVisualizationViewModel
{
    [ObservableProperty] private double _transparency;

    [ObservableProperty] private Color _surfaceColor;
    [ObservableProperty] private Color _edgeColor;
    [ObservableProperty] private Color _axisColor;

    [ObservableProperty] private bool _showSurface;
    [ObservableProperty] private bool _showEdge;
    [ObservableProperty] private bool _showAxis;

    public MockBoundingBoxVisualizationViewModel()
    {
        var faker = new Faker();

        Transparency = faker.Random.Double(0, 100);
        SurfaceColor = Color.FromRgb(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte());
        EdgeColor = Color.FromRgb(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte());
        AxisColor = Color.FromRgb(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte());

        ShowSurface = faker.Random.Bool();
        ShowEdge = faker.Random.Bool();
        ShowAxis = faker.Random.Bool();
    }

    public void RegisterServer(object boundingBoxXyz)
    {
    }

    public void UnregisterServer()
    {
    }
}
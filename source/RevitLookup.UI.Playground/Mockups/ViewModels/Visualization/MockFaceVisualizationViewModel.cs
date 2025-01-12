using System.Windows.Media;
using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using JetBrains.Annotations;
using RevitLookup.Abstractions.ViewModels.Visualization;

namespace RevitLookup.UI.Playground.Mockups.ViewModels.Visualization;

[UsedImplicitly]
public sealed partial class MockFaceVisualizationViewModel : ObservableObject, IFaceVisualizationViewModel
{
    [ObservableProperty] private double _extrusion;
    [ObservableProperty] private double _transparency;

    [ObservableProperty] private Color _surfaceColor;
    [ObservableProperty] private Color _meshColor;
    [ObservableProperty] private Color _normalVectorColor;

    [ObservableProperty] private bool _showSurface;
    [ObservableProperty] private bool _showMeshGrid;
    [ObservableProperty] private bool _showNormalVector;

    public double MinExtrusion { get; }

    public MockFaceVisualizationViewModel()
    {
        var faker = new Faker();

        MinExtrusion = 0;
        Transparency = faker.Random.Double(0, 100);
        Extrusion = faker.Random.Double(0, 24);
        SurfaceColor = Color.FromRgb(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte());
        MeshColor = Color.FromRgb(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte());
        NormalVectorColor = Color.FromRgb(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte());

        ShowSurface = faker.Random.Bool();
        ShowMeshGrid = faker.Random.Bool();
        ShowNormalVector = faker.Random.Bool();
    }

    public void RegisterServer(object face)
    {
    }

    public void UnregisterServer()
    {
    }
}
using RevitLookup.Abstractions.ViewModels.Visualization;

namespace RevitLookup2.ViewModels.Visualization;

[UsedImplicitly]
public sealed partial class MockSolidVisualizationViewModel : ObservableObject, ISolidVisualizationViewModel
{
    [ObservableProperty] private double _scale;
    [ObservableProperty] private double _transparency;

    [ObservableProperty] private Color _faceColor;
    [ObservableProperty] private Color _edgeColor;

    [ObservableProperty] private bool _showFace;
    [ObservableProperty] private bool _showEdge;

    public MockSolidVisualizationViewModel()
    {
        var faker = new Faker();

        Transparency = faker.Random.Double(0, 100);
        Scale = faker.Random.Double(100, 400);
        FaceColor = Color.FromRgb(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte());
        EdgeColor = Color.FromRgb(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte());

        ShowFace = faker.Random.Bool();
        ShowEdge = faker.Random.Bool();
    }

    public void RegisterServer(object solid)
    {
    }

    public void UnregisterServer()
    {
    }
}
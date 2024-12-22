using Microsoft.Extensions.Logging;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.Abstractions.ViewModels.Visualization;
using RevitLookup.Core.Tools.Visualization;
using RevitLookup.Core.Tools.Visualization.Events;
using RevitLookup.UI.Framework.Services.Presentation;
using Color = System.Windows.Media.Color;

namespace RevitLookup.ViewModels.Visualization;

[UsedImplicitly]
public sealed partial class PolylineVisualizationViewModel(
    NotificationService notificationService,
    ISettingsService settingsService,
    ILogger<PolylineVisualizationViewModel> logger)
    : ObservableObject, IPolylineVisualizationViewModel
{
    private readonly PolylineVisualizationServer _server = new();

    [ObservableProperty] private double _diameter = settingsService.RenderSettings.PolylineSettings.Diameter;
    [ObservableProperty] private double _transparency = settingsService.RenderSettings.PolylineSettings.Transparency;

    [ObservableProperty] private Color _surfaceColor = settingsService.RenderSettings.PolylineSettings.SurfaceColor;
    [ObservableProperty] private Color _curveColor = settingsService.RenderSettings.PolylineSettings.CurveColor;
    [ObservableProperty] private Color _directionColor = settingsService.RenderSettings.PolylineSettings.DirectionColor;

    [ObservableProperty] private bool _showSurface = settingsService.RenderSettings.PolylineSettings.ShowSurface;
    [ObservableProperty] private bool _showCurve = settingsService.RenderSettings.PolylineSettings.ShowCurve;
    [ObservableProperty] private bool _showDirection = settingsService.RenderSettings.PolylineSettings.ShowDirection;

    public double MinThickness => settingsService.RenderSettings.PolylineSettings.MinThickness;

    public void RegisterServer(object curveOrEdge)
    {
        Initialize();
        _server.RenderFailed += HandleRenderFailure;

        switch (curveOrEdge)
        {
            case Curve curve:
                _server.Register(curve.Tessellate());
                break;
            case Edge edge:
                _server.Register(edge.Tessellate());
                break;
            default:
                throw new ArgumentException("Unexpected polyline type", nameof(curveOrEdge));
        }
    }

    private void Initialize()
    {
        UpdateShowSurface(ShowSurface);
        UpdateShowCurve(ShowCurve);
        UpdateShowDirection(ShowDirection);

        UpdateSurfaceColor(SurfaceColor);
        UpdateCurveColor(CurveColor);
        UpdateDirectionColor(DirectionColor);

        UpdateTransparency(Transparency);
        UpdateDiameter(Diameter);
    }

    public void UnregisterServer()
    {
        _server.RenderFailed -= HandleRenderFailure;
        _server.Unregister();
    }

    private void HandleRenderFailure(object? sender, RenderFailedEventArgs args)
    {
        logger.LogError(args.ExceptionObject, "Render error");
        notificationService.ShowError("Render error", args.ExceptionObject);
    }

    partial void OnSurfaceColorChanged(Color value)
    {
        settingsService.RenderSettings.PolylineSettings.SurfaceColor = value;
        UpdateSurfaceColor(value);
    }

    partial void OnCurveColorChanged(Color value)
    {
        settingsService.RenderSettings.PolylineSettings.CurveColor = value;
        UpdateCurveColor(value);
    }

    partial void OnDirectionColorChanged(Color value)
    {
        settingsService.RenderSettings.PolylineSettings.DirectionColor = value;
        UpdateDirectionColor(value);
    }

    partial void OnDiameterChanged(double value)
    {
        settingsService.RenderSettings.PolylineSettings.Diameter = value;
        UpdateDiameter(value);
    }

    partial void OnTransparencyChanged(double value)
    {
        settingsService.RenderSettings.PolylineSettings.Transparency = value;
        UpdateTransparency(value);
    }

    partial void OnShowSurfaceChanged(bool value)
    {
        settingsService.RenderSettings.PolylineSettings.ShowSurface = value;
        UpdateShowSurface(value);
    }

    partial void OnShowCurveChanged(bool value)
    {
        settingsService.RenderSettings.PolylineSettings.ShowCurve = value;
        UpdateShowCurve(value);
    }

    partial void OnShowDirectionChanged(bool value)
    {
        settingsService.RenderSettings.PolylineSettings.ShowDirection = value;
        UpdateShowDirection(value);
    }

    private void UpdateSurfaceColor(Color value)
    {
        _server.UpdateSurfaceColor(new Autodesk.Revit.DB.Color(value.R, value.G, value.B));
    }

    private void UpdateCurveColor(Color value)
    {
        _server.UpdateCurveColor(new Autodesk.Revit.DB.Color(value.R, value.G, value.B));
    }

    private void UpdateDirectionColor(Color value)
    {
        _server.UpdateDirectionColor(new Autodesk.Revit.DB.Color(value.R, value.G, value.B));
    }

    private void UpdateDiameter(double value)
    {
        _server.UpdateDiameter(value / 12);
    }

    private void UpdateTransparency(double value)
    {
        _server.UpdateTransparency(value / 100);
    }

    private void UpdateShowSurface(bool value)
    {
        _server.UpdateSurfaceVisibility(value);
    }

    private void UpdateShowCurve(bool value)
    {
        _server.UpdateCurveVisibility(value);
    }

    private void UpdateShowDirection(bool value)
    {
        _server.UpdateDirectionVisibility(value);
    }
}
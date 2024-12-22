using Microsoft.Extensions.Logging;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.Abstractions.ViewModels.Visualization;
using RevitLookup.Core.Tools.Visualization;
using RevitLookup.Core.Tools.Visualization.Events;
using RevitLookup.UI.Framework.Services.Presentation;
using Color = System.Windows.Media.Color;

namespace RevitLookup.ViewModels.Visualization;

[UsedImplicitly]
public sealed partial class SolidVisualizationViewModel(
    NotificationService notificationService,
    ISettingsService settingsService,
    ILogger<SolidVisualizationViewModel> logger)
    : ObservableObject, ISolidVisualizationViewModel
{
    private readonly SolidVisualizationServer _server = new();

    [ObservableProperty] private double _scale = settingsService.RenderSettings.SolidSettings.Scale;
    [ObservableProperty] private double _transparency = settingsService.RenderSettings.SolidSettings.Transparency;

    [ObservableProperty] private Color _faceColor = settingsService.RenderSettings.SolidSettings.FaceColor;
    [ObservableProperty] private Color _edgeColor = settingsService.RenderSettings.SolidSettings.EdgeColor;

    [ObservableProperty] private bool _showFace = settingsService.RenderSettings.SolidSettings.ShowFace;
    [ObservableProperty] private bool _showEdge = settingsService.RenderSettings.SolidSettings.ShowEdge;

    public void RegisterServer(object solidObject)
    {
        if (solidObject is not Solid solid)
        {
            throw new ArgumentException($"Argument must be of type {nameof(Solid)}", nameof(solidObject));
        }

        UpdateShowFace(ShowFace);
        UpdateShowEdge(ShowEdge);

        UpdateFaceColor(FaceColor);
        UpdateEdgeColor(EdgeColor);

        UpdateTransparency(Transparency);
        UpdateScale(Scale);

        _server.RenderFailed += HandleRenderFailure;
        _server.Register(solid);
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

    partial void OnFaceColorChanged(Color value)
    {
        settingsService.RenderSettings.SolidSettings.FaceColor = value;
        UpdateFaceColor(value);
    }

    partial void OnEdgeColorChanged(Color value)
    {
        settingsService.RenderSettings.SolidSettings.EdgeColor = value;
        UpdateEdgeColor(value);
    }

    partial void OnTransparencyChanged(double value)
    {
        settingsService.RenderSettings.SolidSettings.Transparency = value;
        UpdateTransparency(value);
    }

    partial void OnScaleChanged(double value)
    {
        settingsService.RenderSettings.SolidSettings.Scale = value;
        UpdateScale(value);
    }

    partial void OnShowFaceChanged(bool value)
    {
        settingsService.RenderSettings.SolidSettings.ShowFace = value;
        UpdateShowFace(value);
    }

    partial void OnShowEdgeChanged(bool value)
    {
        settingsService.RenderSettings.SolidSettings.ShowEdge = value;
        UpdateShowEdge(value);
    }

    private void UpdateFaceColor(Color value)
    {
        _server.UpdateFaceColor(new Autodesk.Revit.DB.Color(value.R, value.G, value.B));
    }

    private void UpdateEdgeColor(Color value)
    {
        _server.UpdateEdgeColor(new Autodesk.Revit.DB.Color(value.R, value.G, value.B));
    }

    private void UpdateTransparency(double value)
    {
        _server.UpdateTransparency(value / 100);
    }

    private void UpdateScale(double value)
    {
        _server.UpdateScale(value / 100);
    }

    private void UpdateShowFace(bool value)
    {
        _server.UpdateFaceVisibility(value);
    }

    private void UpdateShowEdge(bool value)
    {
        _server.UpdateEdgeVisibility(value);
    }
}
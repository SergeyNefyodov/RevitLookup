using RevitLookup.Abstractions.Models.Settings;

namespace RevitLookup.Abstractions.Services.Settings;

public interface ISettingsService
{
    public GeneralSettings GeneralSettings { get; }
    public RenderSettings RenderSettings { get; }
    void SaveSettings();
    void LoadSettings();
    void ResetGeneralSettings();
    void ResetRenderSettings();
}
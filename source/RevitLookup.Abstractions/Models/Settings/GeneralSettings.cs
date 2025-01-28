using System.Text.Json.Serialization;
using Wpf.Ui.Animations;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitLookup.Abstractions.Models.Settings;

[Serializable]
public sealed class GeneralSettings
{
    [JsonPropertyName("Theme")] public ApplicationTheme Theme { get; set; }
    [JsonPropertyName("Background")] public WindowBackdropType Background { get; set; }
    [JsonPropertyName("Transition")] public Transition Transition { get; set; }

    [JsonPropertyName("WindowWidth")] public double WindowWidth { get; set; }
    [JsonPropertyName("WindowHeight")] public double WindowHeight { get; set; }

    [JsonPropertyName("IsPrivateAllowed")] public bool IncludePrivate { get; set; }
    [JsonPropertyName("IsFieldsAllowed")] public bool IncludeFields { get; set; }
    [JsonPropertyName("IsStaticAllowed")] public bool IncludeStatic { get; set; }
    [JsonPropertyName("IsEventsAllowed")] public bool IncludeEvents { get; set; }
    [JsonPropertyName("IsExtensionsAllowed")] public bool IncludeExtensions { get; set; }
    [JsonPropertyName("IsUnsupportedAllowed")] public bool IncludeUnsupported { get; set; }
    [JsonPropertyName("IsRootHierarchyAllowed")] public bool IncludeRootHierarchy { get; set; }

    [JsonPropertyName("IsHardwareRenderingAllowed")] public bool UseHardwareRendering { get; set; }
    [JsonPropertyName("IsTimeColumnAllowed")] public bool ShowTimeColumn { get; set; }
    [JsonPropertyName("ShowMemoryColumn")] public bool ShowMemoryColumn { get; set; }
    [JsonPropertyName("UseSizeRestoring")] public bool UseSizeRestoring { get; set; }
    [JsonPropertyName("IsModifyTabAllowed")] public bool UseModifyTab { get; set; }
}
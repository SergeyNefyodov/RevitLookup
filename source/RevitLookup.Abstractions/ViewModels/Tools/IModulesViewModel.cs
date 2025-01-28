using RevitLookup.Abstractions.Models.Tools;

namespace RevitLookup.Abstractions.ViewModels.Tools;

public interface IModulesViewModel
{
    string SearchText { get; set; }
    List<ModuleInfo> FilteredModules { get; set; }
    List<ModuleInfo> Modules { get; set; }
}
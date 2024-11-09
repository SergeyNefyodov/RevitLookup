using RevitLookup.Abstractions.Models.Tools;

namespace RevitLookup.Abstractions.ViewModels.Tools;

public interface IUnitsViewModel
{
    List<UnitInfo> Units { get; set; }
    List<UnitInfo> FilteredUnits { get; set; }
    string SearchText { get; set; }
    void InitializeParameters();
    void InitializeCategories();
    void InitializeForgeSchema();
}
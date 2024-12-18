using RevitLookup.Abstractions.Services;
using RevitLookup.Abstractions.ViewModels.Tools;
using RevitLookup2.Core.Tools;

namespace RevitLookup2.ViewModels.Tools;

[UsedImplicitly]
public sealed partial class SearchElementsViewModel(
    INotificationService notificationService,
    IVisualDecompositionService decompositionService)
    : ObservableObject, ISearchElementsViewModel
{
    [ObservableProperty] private string _searchText = string.Empty;

    public async Task<bool> SearchElementsAsync()
    {
        var result = SearchText != string.Empty;
        if (result)
        {
            var elements = ElementsFinder.SearchElements(SearchText);
            await decompositionService.VisualizeDecompositionAsync(elements);
        }
        else
        {
            notificationService.ShowWarning("Search elements", "There are no elements found for your request");
        }

        return result;
    }
}
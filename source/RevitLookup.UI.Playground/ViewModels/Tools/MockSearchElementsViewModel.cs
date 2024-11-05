using CommunityToolkit.Mvvm.ComponentModel;
using RevitLookup.Abstractions.Services;
using RevitLookup.Abstractions.ViewModels.Tools;

namespace RevitLookup.UI.Playground.ViewModels.Tools;

public sealed partial class MockSearchElementsViewModel(INotificationService notificationService) : ObservableObject, ISearchElementsViewModel
{
    [ObservableProperty] private string _searchText = string.Empty;

    public bool SearchElements()
    {
        var result = SearchText != string.Empty;

        if (!result)
        {
            notificationService.ShowWarning("Search elements", "There are no elements found for your request");
        }

        return result;
    }
}
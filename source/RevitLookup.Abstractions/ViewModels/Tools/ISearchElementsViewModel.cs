namespace RevitLookup.Abstractions.ViewModels.Tools;

public interface ISearchElementsViewModel
{
    string SearchText { get; set; }
    bool SearchElements();
}
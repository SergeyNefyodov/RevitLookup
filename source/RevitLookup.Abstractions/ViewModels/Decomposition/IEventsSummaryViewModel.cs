using System.Collections.ObjectModel;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.Abstractions.ViewModels.Decomposition;

public interface IEventsSummaryViewModel : ISummaryViewModel, INavigationAware
{
    ObservableCollection<ObservableDecomposedObject> FilteredDecomposedObjects { get; }
}
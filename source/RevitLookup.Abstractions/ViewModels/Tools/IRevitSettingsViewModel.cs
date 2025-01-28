using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using RevitLookup.Abstractions.ObservableModels.Entries;

namespace RevitLookup.Abstractions.ViewModels.Tools;

public interface IRevitSettingsViewModel
{
    //Filter
    bool Filtered { get; set; }
    string CategoryFilter { get; set; }
    string PropertyFilter { get; set; }
    string ValueFilter { get; set; }
    bool ShowUserSettingsFilter { get; set; }

    //Items
    ObservableIniEntry? SelectedEntry { get; set; }
    List<ObservableIniEntry> Entries { get; set; }
    ObservableCollection<ObservableIniEntry> FilteredEntries { get; set; }

    //Commands
    IRelayCommand ShowHelpCommand { get; }
    IRelayCommand OpenSettingsCommand { get; }
    IRelayCommand ClearFiltersCommand { get; }
    IAsyncRelayCommand CreateEntryCommand { get; }
    IRelayCommand<ObservableIniEntry> ActivateEntryCommand { get; }
    IRelayCommand<ObservableIniEntry> DeleteEntryCommand { get; }
    IRelayCommand<ObservableIniEntry> RestoreDefaultCommand { get; }

    Task<List<ObservableIniEntry>>? InitializationTask { get; }
    Task InitializeAsync();
    Task UpdateEntryAsync();
}
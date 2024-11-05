using CommunityToolkit.Mvvm.Input;
using RevitLookup.Abstractions.States;

namespace RevitLookup.Abstractions.ViewModels.AboutProgram;

public interface IAboutViewModel
{
    SoftwareUpdateState State { get; set; }
    bool IsUpdateChecked { get; set; }
    Version CurrentVersion { get; set; }
    string NewVersion { get; set; }
    string ErrorMessage { get; set; }
    string ReleaseNotesUrl { get; set; }
    string LatestCheckDate { get; set; }
    string Runtime { get; set; }

    IAsyncRelayCommand CheckUpdatesCommand { get; }
    IAsyncRelayCommand DownloadUpdateCommand { get; }
    IAsyncRelayCommand ShowSoftwareDialogCommand { get; }
}
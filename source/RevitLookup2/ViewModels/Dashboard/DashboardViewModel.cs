using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Abstractions.Models.Summary;
using RevitLookup.Abstractions.Models.UserInterface;
using RevitLookup.Abstractions.Services;
using RevitLookup.Abstractions.ViewModels.Dashboard;
using RevitLookup.UI.Framework.Views.Summary;
using RevitLookup.UI.Framework.Views.Tools;
using Wpf.Ui;
using Wpf.Ui.Controls;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;

namespace RevitLookup2.ViewModels.Dashboard;

[UsedImplicitly]
public sealed partial class DashboardViewModel : IDashboardViewModel
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationService _navigationService;
    private readonly INotificationService _notificationService;
    private readonly IVisualDecompositionService _visualDecompositionService;

    public DashboardViewModel(
        IServiceProvider serviceProvider,
        INavigationService navigationService,
        INotificationService notificationService,
        IVisualDecompositionService visualDecompositionService)
    {
        _serviceProvider = serviceProvider;
        _navigationService = navigationService;
        _notificationService = notificationService;
        _visualDecompositionService = visualDecompositionService;

        NavigationGroups =
        [
            new NavigationCardGroup
            {
                GroupName = "Workspace",
                Items =
                [
                    new NavigationCardItem
                    {
                        Title = "Active view",
                        Description = "Explore the active view, which defines how the model is presented visually",
                        Icon = SymbolRegular.Image24,
                        Command = NavigatePageCommand,
                        CommandParameter = "view"
                    },
                    new NavigationCardItem
                    {
                        Title = "Active document",
                        Description = "Explore the document currently open in the Revit session",
                        Icon = SymbolRegular.Document24,
                        Command = NavigatePageCommand,
                        CommandParameter = "document"
                    },
                    new NavigationCardItem
                    {
                        Title = "Application",
                        Description = "Explore the application object, providing access to application wide data and settings",
                        Icon = SymbolRegular.Apps24,
                        Command = NavigatePageCommand,
                        CommandParameter = "application"
                    },
                    new NavigationCardItem
                    {
                        Title = "UI Application",
                        Description = "Explore an active session of the Autodesk Revit user interface and its customization options",
                        Icon = SymbolRegular.WindowApps24,
                        Command = NavigatePageCommand,
                        CommandParameter = "uiApplication"
                    },
                    new NavigationCardItem
                    {
                        Title = "Database",
                        Description = "Explore the Revit model database containing all elements and their relationships",
                        Icon = SymbolRegular.Database24,
                        Command = NavigatePageCommand,
                        CommandParameter = "database"
                    },
                    new NavigationCardItem
                    {
                        Title = "Dependent elements",
                        Description = "Explore the children of the selected elements",
                        Icon = SymbolRegular.DataLine24,
                        Command = NavigatePageCommand,
                        CommandParameter = "dependents"
                    }
                ]
            },
            new NavigationCardGroup
            {
                GroupName = "Interaction",
                Items =
                [
                    new NavigationCardItem
                    {
                        Title = "Selection",
                        Description = "Explore the currently selected elements in the model",
                        Icon = SymbolRegular.SquareHint24,
                        Command = NavigatePageCommand,
                        CommandParameter = "selection"
                    },
                    new NavigationCardItem
                    {
                        Title = "Linked element",
                        Description = "Select and explore an element from linked model",
                        Icon = SymbolRegular.LinkSquare24,
                        Command = NavigatePageCommand,
                        CommandParameter = "linked"
                    },
                    new NavigationCardItem
                    {
                        Title = "Face",
                        Description = "Select and explore a face of the element's geometry",
                        Icon = SymbolRegular.LayerDiagonal20,
                        Command = NavigatePageCommand,
                        CommandParameter = "face"
                    },
                    new NavigationCardItem
                    {
                        Title = "Edge",
                        Description = "Select and explore the edge of the element's geometry",
                        Icon = SymbolRegular.Line24,
                        Command = NavigatePageCommand,
                        CommandParameter = "edge"
                    },
                    new NavigationCardItem
                    {
                        Title = "Point",
                        Description = "Select and explore a point in the model, such as specific location or coordinate",
                        Icon = SymbolRegular.Location24,
                        Command = NavigatePageCommand,
                        CommandParameter = "point"
                    },
                    new NavigationCardItem
                    {
                        Title = "Sub-element",
                        Description = "Select and explore a sub-element, such as a part or detail of an element",
                        Icon = SymbolRegular.Subtitles24,
                        Command = NavigatePageCommand,
                        CommandParameter = "subElement"
                    }
                ]
            },
            new NavigationCardGroup
            {
                GroupName = "Maintenance",
                Items =
                [
                    new NavigationCardItem
                    {
                        Title = "Component manager",
                        Description = "Explore the component manager, managing the low-level visual representation of Revit",
                        Icon = SymbolRegular.SlideTextMultiple32,
                        Command = NavigatePageCommand,
                        CommandParameter = "components"
                    },
                    new NavigationCardItem
                    {
                        Title = "Performance adviser",
                        Description = "Explore a tool to report performance problems in the active document",
                        Icon = SymbolRegular.HeartPulse24,
                        Command = NavigatePageCommand,
                        CommandParameter = "performance"
                    }
                ]
            },
            new NavigationCardGroup
            {
                GroupName = "Registry",
                Items =
                [
                    new NavigationCardItem
                    {
                        Title = "Updaters",
                        Description = "Explore the storage of all updaters registered in the current session",
                        Icon = SymbolRegular.Whiteboard24,
                        Command = NavigatePageCommand,
                        CommandParameter = "updaters"
                    },
                    new NavigationCardItem
                    {
                        Title = "Schemas",
                        Description = "Explore the memory storage of all schemas registered in the Extensible Storage framework",
                        Icon = SymbolRegular.Box24,
                        Command = NavigatePageCommand,
                        CommandParameter = "schemas"
                    },
                    new NavigationCardItem
                    {
                        Title = "Services",
                        Description = "Explore the services that provide extended functionality in Revit",
                        Icon = SymbolRegular.WeatherCloudy24,
                        Command = NavigatePageCommand,
                        CommandParameter = "services"
                    }
                ]
            },
            new NavigationCardGroup
            {
                GroupName = "Units",
                Items =
                [
                    new NavigationCardItem
                    {
                        Title = "BuiltIn Parameters",
                        Description = "Explore parameters predefined in Revit",
                        Icon = SymbolRegular.LeafOne24,
                        Command = OpenDialogCommand,
                        CommandParameter = "parameters"
                    },
                    new NavigationCardItem
                    {
                        Title = "BuiltIn Categories",
                        Description = "Explore categories predefined in Revit",
                        Icon = SymbolRegular.LeafTwo24,
                        Command = OpenDialogCommand,
                        CommandParameter = "categories"
                    },
                    new NavigationCardItem
                    {
                        Title = "Forge Schema",
                        Description = "Explore Forge schema definitions used in Revit",
                        Icon = SymbolRegular.LeafThree24,
                        Command = OpenDialogCommand,
                        CommandParameter = "forge"
                    }
                ]
            },
            new NavigationCardGroup
            {
                GroupName = "Tools",
                Items =
                [
                    new NavigationCardItem
                    {
                        Title = "Search elements",
                        Description = "Search specific elements in Revit",
                        Icon = SymbolRegular.SlideSearch24,
                        Command = OpenDialogCommand,
                        CommandParameter = "search"
                    },
                    new NavigationCardItem
                    {
                        Title = "Event monitor",
                        Description = "Monitor all incoming events in a Revit session",
                        Icon = SymbolRegular.DesktopPulse24,
                        Command = NavigatePageCommand,
                        CommandParameter = "events"
                    },
                    new NavigationCardItem
                    {
                        Title = "Revit settings",
                        Description = "Inspect configuration and Revit settings available in the application",
                        Icon = SymbolRegular.LauncherSettings24,
                        Command = NavigatePageCommand,
                        CommandParameter = "revitSettings"
                    },
                    new NavigationCardItem
                    {
                        Title = "Modules",
                        Description = "Inspect the dynamic link libraries (DLLs) and executables that Revit uses",
                        Icon = SymbolRegular.BroadActivityFeed24,
                        Command = OpenDialogCommand,
                        CommandParameter = "modules"
                    }
                ]
            }
        ];
    }

    public List<NavigationCardGroup> NavigationGroups { get; }

    [RelayCommand]
    private async Task NavigatePage(string? parameter)
    {
        if (!ValidateContext()) return;

        try
        {
            switch (parameter)
            {
                case "view":
                    await _visualDecompositionService.VisualizeDecompositionAsync(KnownDecompositionObject.View);
                    _navigationService.Navigate(typeof(DecompositionSummaryPage));
                    break;
                case "document":
                    await _visualDecompositionService.VisualizeDecompositionAsync(KnownDecompositionObject.Document);
                    _navigationService.Navigate(typeof(DecompositionSummaryPage));
                    break;
                case "application":
                    await _visualDecompositionService.VisualizeDecompositionAsync(KnownDecompositionObject.Application);
                    _navigationService.Navigate(typeof(DecompositionSummaryPage));
                    break;
                case "uiApplication":
                    await _visualDecompositionService.VisualizeDecompositionAsync(KnownDecompositionObject.UiApplication);
                    _navigationService.Navigate(typeof(DecompositionSummaryPage));
                    break;
                case "database":
                    await _visualDecompositionService.VisualizeDecompositionAsync(KnownDecompositionObject.Database);
                    _navigationService.Navigate(typeof(DecompositionSummaryPage));
                    break;
                case "dependents":
                    await _visualDecompositionService.VisualizeDecompositionAsync(KnownDecompositionObject.DependentElements);
                    _navigationService.Navigate(typeof(DecompositionSummaryPage));
                    break;
                case "selection":
                    await _visualDecompositionService.VisualizeDecompositionAsync(KnownDecompositionObject.Selection);
                    _navigationService.Navigate(typeof(DecompositionSummaryPage));
                    break;
                case "linked":
                    await _visualDecompositionService.VisualizeDecompositionAsync(KnownDecompositionObject.LinkedElement);
                    _navigationService.Navigate(typeof(DecompositionSummaryPage));
                    break;
                case "face":
                    await _visualDecompositionService.VisualizeDecompositionAsync(KnownDecompositionObject.Face);
                    _navigationService.Navigate(typeof(DecompositionSummaryPage));
                    break;
                case "edge":
                    await _visualDecompositionService.VisualizeDecompositionAsync(KnownDecompositionObject.Edge);
                    _navigationService.Navigate(typeof(DecompositionSummaryPage));
                    break;
                case "point":
                    await _visualDecompositionService.VisualizeDecompositionAsync(KnownDecompositionObject.Point);
                    _navigationService.Navigate(typeof(DecompositionSummaryPage));
                    break;
                case "subElement":
                    await _visualDecompositionService.VisualizeDecompositionAsync(KnownDecompositionObject.SubElement);
                    _navigationService.Navigate(typeof(DecompositionSummaryPage));
                    break;
                case "components":
                    await _visualDecompositionService.VisualizeDecompositionAsync(KnownDecompositionObject.ComponentManager);
                    _navigationService.Navigate(typeof(DecompositionSummaryPage));
                    break;
                case "performance":
                    await _visualDecompositionService.VisualizeDecompositionAsync(KnownDecompositionObject.PerformanceAdviser);
                    _navigationService.Navigate(typeof(DecompositionSummaryPage));
                    break;
                case "updaters":
                    await _visualDecompositionService.VisualizeDecompositionAsync(KnownDecompositionObject.UpdaterRegistry);
                    _navigationService.Navigate(typeof(DecompositionSummaryPage));
                    break;
                case "services":
                    await _visualDecompositionService.VisualizeDecompositionAsync(KnownDecompositionObject.Services);
                    _navigationService.Navigate(typeof(DecompositionSummaryPage));
                    break;
                case "schemas":
                    await _visualDecompositionService.VisualizeDecompositionAsync(KnownDecompositionObject.Schemas);
                    _navigationService.Navigate(typeof(DecompositionSummaryPage));
                    break;
                case "events":
                    _navigationService.Navigate(typeof(EventsSummaryPage));
                    break;
                case "revitSettings":
                    _navigationService.NavigateWithHierarchy(typeof(RevitSettingsPage));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(parameter), parameter);
            }
        }
        catch (Exception exception)
        {
            _notificationService.ShowError("Failed to run tool", exception);
        }
    }

    [RelayCommand]
    private async Task OpenDialog(string parameter)
    {
        try
        {
            if (!ValidateContext()) return;

            switch (parameter)
            {
                case "parameters":
                    var unitsDialog = _serviceProvider.GetRequiredService<UnitsDialog>();
                    await unitsDialog.ShowParametersDialogAsync();
                    return;
                case "categories":
                    unitsDialog = _serviceProvider.GetRequiredService<UnitsDialog>();
                    await unitsDialog.ShowCategoriesDialogAsync();
                    return;
                case "forge":
                    unitsDialog = _serviceProvider.GetRequiredService<UnitsDialog>();
                    await unitsDialog.ShowForgeSchemaDialogAsync();
                    return;
                case "search":
                    var searchDialog = _serviceProvider.GetRequiredService<SearchElementsDialog>();
                    await searchDialog.ShowAsync();
                    return;
                case "modules":
                    var modulesDialog = _serviceProvider.GetRequiredService<ModulesDialog>();
                    await modulesDialog.ShowAsync();
                    return;
            }
        }
        catch (Exception exception)
        {
            _notificationService.ShowError("Failed to open dialog", exception);
        }
    }

    //TODO: allow context independent commands
    private bool ValidateContext()
    {
        if (Context.ActiveUiDocument is not null) return true;

        _notificationService.ShowWarning("Invalid context", "There are no open documents");
        return false;
    }
}
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using RevitLookup.Abstractions.Models.UserInterface;
using RevitLookup.Abstractions.ViewModels.Dashboard;
using Wpf.Ui.Controls;

namespace RevitLookup.UI.Playground.ViewModels.Dashboard;

[UsedImplicitly]
public sealed partial class MockDashboardViewModel : IDashboardViewModel
{
    public MockDashboardViewModel()
    {
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
        await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task OpenDialog(string parameter)
    {
        await Task.CompletedTask;
    }
}
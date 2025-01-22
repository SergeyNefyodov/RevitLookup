using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Abstractions.Models.Decomposition;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using RevitLookup.Abstractions.Services.Application;
using RevitLookup.Abstractions.Services.Decomposition;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.UI.Framework.Views.Windows;
using Wpf.Ui;

namespace RevitLookup.UI.Playground.Mockups.Services.Application;

public sealed class MockRevitLookupUiService : IRevitLookupUiService, ILookupServiceParentStage, ILookupServiceRunStage
{
    private IServiceProvider? _parentProvider;
    private readonly Task _activeTask = Task.CompletedTask;
    private readonly IServiceScope _scope;
    private readonly IDecompositionService _decompositionService;
    private readonly IVisualDecompositionService _visualDecompositionService;
    private readonly INavigationService _navigationService;
    private readonly Window _host;

    public MockRevitLookupUiService(IServiceScopeFactory scopeFactory)
    {
        _scope = scopeFactory.CreateScope();

        _host = _scope.ServiceProvider.GetRequiredService<RevitLookupView>();
        _decompositionService = _scope.ServiceProvider.GetRequiredService<IDecompositionService>();
        _visualDecompositionService = _scope.ServiceProvider.GetRequiredService<IVisualDecompositionService>();
        _navigationService = _scope.ServiceProvider.GetRequiredService<INavigationService>();

        _host.Closed += (_, _) => _scope.Dispose();
    }

    public ILookupServiceShowStage Decompose(KnownDecompositionObject decompositionObject)
    {
        _activeTask.ContinueWith(_ => _visualDecompositionService.VisualizeDecompositionAsync(decompositionObject), TaskScheduler.FromCurrentSynchronizationContext());
        return this;
    }

    public ILookupServiceShowStage Decompose(object? obj)
    {
        _activeTask.ContinueWith(_ => _visualDecompositionService.VisualizeDecompositionAsync(obj), TaskScheduler.FromCurrentSynchronizationContext());
        return this;
    }

    public ILookupServiceShowStage Decompose(IEnumerable objects)
    {
        _activeTask.ContinueWith(_ => _visualDecompositionService.VisualizeDecompositionAsync(objects), TaskScheduler.FromCurrentSynchronizationContext());
        return this;
    }

    public ILookupServiceShowStage Decompose(ObservableDecomposedObject decomposedObject)
    {
        _activeTask.ContinueWith(_ => _visualDecompositionService.VisualizeDecompositionAsync(decomposedObject), TaskScheduler.FromCurrentSynchronizationContext());
        return this;
    }

    public ILookupServiceShowStage Decompose(List<ObservableDecomposedObject> decomposedObjects)
    {
        _activeTask.ContinueWith(_ => _visualDecompositionService.VisualizeDecompositionAsync(decomposedObjects), TaskScheduler.FromCurrentSynchronizationContext());
        return this;
    }

    public ILookupServiceParentStage AddParent(IServiceProvider parentProvider)
    {
        _parentProvider = parentProvider;

        var decompositionService = parentProvider.GetRequiredService<IDecompositionService>();
        _decompositionService.DecompositionStackHistory.AddRange(decompositionService.DecompositionStackHistory);
        return this;
    }

    public ILookupServiceDecomposeStage AddStackHistory(ObservableDecomposedObject item)
    {
        _decompositionService.DecompositionStackHistory.Add(item);
        return this;
    }

    public ILookupServiceRunStage Show<T>() where T : Page
    {
        _activeTask.ContinueWith(_ =>
        {
            ShowHost(false);
            _navigationService.Navigate(typeof(T));
        }, TaskScheduler.FromCurrentSynchronizationContext());

        return this;
    }

    public void RunService<T>(Action<T> handler) where T : class
    {
        _activeTask.ContinueWith(_ => InvokeService(handler), TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void InvokeService<T>(Action<T> handler) where T : class
    {
        var service = _scope.ServiceProvider.GetRequiredService<T>();
        handler.Invoke(service);
    }

    private void ShowHost(bool modal)
    {
        if (_parentProvider is null)
        {
            _host.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        else
        {
            var parentHost = _parentProvider.GetRequiredService<IWindowIntercomService>().GetHost();

            _host.WindowStartupLocation = WindowStartupLocation.Manual;
            _host.Left = parentHost.Left + 47;
            _host.Top = parentHost.Top + 49;
        }

        if (modal)
        {
            _host.ShowDialog();
        }
        else
        {
            _host.Show();
            _host.Focus();
        }
    }
}
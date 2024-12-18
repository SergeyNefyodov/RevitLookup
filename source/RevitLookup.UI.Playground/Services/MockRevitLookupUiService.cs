using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Abstractions.Models.Summary;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using RevitLookup.Abstractions.Services;
using RevitLookup.UI.Framework.Views.Windows;
using Wpf.Ui;

namespace RevitLookup.UI.Playground.Services;

public sealed class MockRevitLookupUiService : IRevitLookupUiService
{
    private Window? _parent;
    private readonly Task _activeTask = Task.CompletedTask;
    private readonly IServiceScope _scope;
    private readonly IVisualDecompositionService _decompositionService;
    private readonly INavigationService _navigationService;
    private readonly Window _host;

    public MockRevitLookupUiService(IServiceScopeFactory scopeFactory)
    {
        _scope = scopeFactory.CreateScope();

        _host = _scope.ServiceProvider.GetRequiredService<RevitLookupView>();
        _decompositionService = _scope.ServiceProvider.GetRequiredService<IVisualDecompositionService>();
        _navigationService = _scope.ServiceProvider.GetRequiredService<INavigationService>();

        _host.Closed += (_, _) => _scope.Dispose();
    }

    public ILookupServiceDependsStage Decompose(KnownDecompositionObject decompositionObject)
    {
        _activeTask.ContinueWith(_ => _decompositionService.VisualizeDecompositionAsync(decompositionObject), TaskScheduler.FromCurrentSynchronizationContext());
        return this;
    }

    public ILookupServiceDependsStage Decompose(object? obj)
    {
        _activeTask.ContinueWith(_ => _decompositionService.VisualizeDecompositionAsync(obj), TaskScheduler.FromCurrentSynchronizationContext());
        return this;
    }

    public ILookupServiceDependsStage Decompose(IEnumerable objects)
    {
        _activeTask.ContinueWith(_ => _decompositionService.VisualizeDecompositionAsync(objects), TaskScheduler.FromCurrentSynchronizationContext());
        return this;
    }

    public ILookupServiceDependsStage Decompose(ObservableDecomposedObject decomposedObject)
    {
        _activeTask.ContinueWith(_ => _decompositionService.VisualizeDecompositionAsync(decomposedObject), TaskScheduler.FromCurrentSynchronizationContext());
        return this;
    }

    public ILookupServiceDependsStage Decompose(List<ObservableDecomposedObject> decomposedObjects)
    {
        _activeTask.ContinueWith(_ => _decompositionService.VisualizeDecompositionAsync(decomposedObjects), TaskScheduler.FromCurrentSynchronizationContext());
        return this;
    }

    public ILookupServiceShowStage DependsOn(Window parent)
    {
        _parent = parent;
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
        if (_parent is null)
        {
            _host.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        else
        {
            _host.WindowStartupLocation = WindowStartupLocation.Manual;
            _host.Left = _parent.Left + 47;
            _host.Top = _parent.Top + 49;
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
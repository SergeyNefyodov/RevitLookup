namespace RevitLookup.UI.Playground.Services;

// public sealed class RevitLookupUiService : IRevitLookupUiService
// {
//     private static readonly Dispatcher Dispatcher;
//     private UiServiceImpl _uiService = default!;
//
//     static RevitLookupUiService()
//     {
//         var uiThread = new Thread(Dispatcher.Run);
//         uiThread.SetApartmentState(ApartmentState.STA);
//         uiThread.Start();
//
//         Dispatcher = EnsureDispatcherStart(uiThread);
//     }
//
//     public RevitLookupUiService(IServiceScopeFactory scopeFactory)
//     {
//         if (Dispatcher.CheckAccess())
//         {
//             _uiService = new UiServiceImpl(scopeFactory);
//         }
//         else
//         {
//             Dispatcher.Invoke(() => _uiService = new UiServiceImpl(scopeFactory));
//         }
//     }
//     
//     public ILookupServiceDependsStage Decompose(KnownDecompositionObject decompositionObject)
//     {
//         if (Dispatcher.CheckAccess())
//         {
//             _uiService.Decompose(decompositionObject);
//         }
//         else
//         {
//             Dispatcher.Invoke(() => _uiService.Decompose(decompositionObject));
//         }
//         
//         return this;
//     }
//     public ILookupServiceDependsStage Decompose(object obj)
//     {
//         if (Dispatcher.CheckAccess())
//         {
//             _uiService.Decompose(obj);
//         }
//         else
//         {
//             Dispatcher.Invoke(() => _uiService.Decompose(obj));
//         }
//         
//         return this;
//     }
//
//     public ILookupServiceDependsStage Decompose(IEnumerable objects)
//     {
//         if (Dispatcher.CheckAccess())
//         {
//             _uiService.Decompose(objects);
//         }
//         else
//         {
//             Dispatcher.Invoke(() => _uiService.Decompose(objects));
//         }
//         
//         return this;
//     }
//
//     public ILookupServiceDependsStage Decompose(ObservableDecomposedObject decomposedObject)
//     {
//         if (Dispatcher.CheckAccess())
//         {
//             _uiService.Decompose(decomposedObject);
//         }
//         else
//         {
//             Dispatcher.Invoke(() => _uiService.Decompose(decomposedObject));
//         }
//         
//         return this;
//     }
//
//     public ILookupServiceDependsStage Decompose(List<ObservableDecomposedObject> decomposedObjects)
//     {
//         if (Dispatcher.CheckAccess())
//         {
//             _uiService.Decompose(decomposedObjects);
//         }
//         else
//         {
//             Dispatcher.Invoke(() => _uiService.Decompose(decomposedObjects));
//         }
//         
//         return this;
//     }
//     
//     public ILookupServiceShowStage DependsOn(Window parent)
//     {
//         if (Dispatcher.CheckAccess())
//         {
//             _uiService.DependsOn(parent);
//         }
//         else
//         {
//             Dispatcher.Invoke(() => _uiService.DependsOn(parent));
//         }
//         
//         return this;
//     }
//     
//     public ILookupServiceRunStage Show<T>() where T : Page
//     {
//         if (Dispatcher.CheckAccess())
//         {
//             _uiService.Show<T>();
//         }
//         else
//         {
//             Dispatcher.Invoke(() => _uiService.Show<T>());
//         }
//         
//         return this;
//     }
//     
//     public void RunService<T>(Action<T> handler) where T : class
//     {
//         if (Dispatcher.CheckAccess())
//         {
//             _uiService.RunService(handler);
//         }
//         else
//         {
//             Dispatcher.Invoke(() => _uiService.RunService(handler));
//         }
//     }
//
//     private static Dispatcher EnsureDispatcherStart(Thread thread)
//     {
//         Dispatcher? dispatcher = null;
//         SpinWait spinWait = new();
//         while (dispatcher is null)
//         {
//             spinWait.SpinOnce();
//             dispatcher = Dispatcher.FromThread(thread);
//         }
//
//         // We must yield
//         // Sometimes the Dispatcher is unavailable for current thread
//         Thread.Sleep(1);
//
//         return dispatcher;
//     }
//
//     private sealed class UiServiceImpl
//     {
//         private Window? _parent;
//         private Task? _activeTask;
//         private readonly IServiceScope _scope;
//         private readonly IVisualDecompositionService _decompositionService;
//         private readonly INavigationService _navigationService;
//         private readonly Window _host;
//
//         public UiServiceImpl(IServiceScopeFactory scopeFactory)
//         {
//             _scope = scopeFactory.CreateScope();
//
//             _host = _scope.ServiceProvider.GetRequiredService<RevitLookupView>();
//             _decompositionService = _scope.ServiceProvider.GetRequiredService<IVisualDecompositionService>();
//             _navigationService = _scope.ServiceProvider.GetRequiredService<INavigationService>();
//
//             _host.Closed += (_, _) => _scope.Dispose();
//         }
//
//         public void Decompose(KnownDecompositionObject decompositionObject)
//         {
//             if (_activeTask is null || _activeTask.IsCompleted)
//             {
//                 _activeTask = _decompositionService.VisualizeDecompositionAsync(decompositionObject);
//             }
//             else
//             {
//                 _activeTask = _activeTask.ContinueWith(_ => _decompositionService.VisualizeDecompositionAsync(decompositionObject), TaskScheduler.FromCurrentSynchronizationContext());
//             }
//         }
//
//         public void Decompose(object obj)
//         {
//             if (_activeTask is null || _activeTask.IsCompleted)
//             {
//                 _activeTask = _decompositionService.VisualizeDecompositionAsync(obj);
//             }
//             else
//             {
//                 _activeTask = _activeTask.ContinueWith(_ => _decompositionService.VisualizeDecompositionAsync(obj), TaskScheduler.FromCurrentSynchronizationContext());
//             }
//         }
//
//         public void Decompose(IEnumerable enumerable)
//         {
//             if (_activeTask is null || _activeTask.IsCompleted)
//             {
//                 _activeTask = _decompositionService.VisualizeDecompositionAsync(enumerable);
//             }
//             else
//             {
//                 _activeTask = _activeTask.ContinueWith(_ => _decompositionService.VisualizeDecompositionAsync(enumerable), TaskScheduler.FromCurrentSynchronizationContext());
//             }
//         }
//
//         public void Decompose(ObservableDecomposedObject decomposedObject)
//         {
//             _decompositionService.VisualizeDecompositionAsync(decomposedObject);
//         }
//
//         public void Decompose(List<ObservableDecomposedObject> decomposedObjects)
//         {
//             _decompositionService.VisualizeDecompositionAsync(decomposedObjects);
//         }
//
//         public void DependsOn(Window parent)
//         {
//             _parent = parent;
//         }
//
//         public void Show<T>() where T : Page
//         {
//             if (_activeTask is null || _activeTask.IsCompleted)
//             {
//                 ShowHost(false);
//                 _navigationService.Navigate(typeof(T));
//             }
//             else
//             {
//                 _activeTask = _activeTask.ContinueWith(_ =>
//                 {
//                     ShowHost(false);
//                     _navigationService.Navigate(typeof(T));
//                 }, TaskScheduler.FromCurrentSynchronizationContext());
//             }
//         }
//
//         public void RunService<T>(Action<T> handler) where T : class
//         {
//             if (_activeTask is null || _activeTask.IsCompleted)
//             {
//                 InvokeService(handler);
//             }
//             else
//             {
//                 _activeTask = _activeTask.ContinueWith(_ => InvokeService(handler), TaskScheduler.FromCurrentSynchronizationContext());
//             }
//         }
//
//         private void InvokeService<T>(Action<T> handler) where T : class
//         {
//             var service = _scope.ServiceProvider.GetRequiredService<T>();
//             handler.Invoke(service);
//         }
//
//         private void ShowHost(bool modal)
//         {
//             if (_parent is null)
//             {
//                 _host.WindowStartupLocation = WindowStartupLocation.CenterScreen;
//             }
//             else
//             {
//                 _host.WindowStartupLocation = WindowStartupLocation.Manual;
//                 _host.Left = _parent.Left + 47;
//                 _host.Top = _parent.Top + 49;
//             }
//
//             if (modal)
//             {
//                 _host.ShowDialog();
//             }
//             else
//             {
//                 _host.Show();
//             }
//         }
//     }
// }
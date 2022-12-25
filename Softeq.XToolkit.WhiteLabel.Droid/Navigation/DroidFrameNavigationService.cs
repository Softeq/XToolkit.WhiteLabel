// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using AndroidX.Fragment.App;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Droid.Internal;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class DroidFrameNavigationService : IFrameNavigationService
    {
        private readonly BackStack<IViewModelBase> _backStack = new BackStack<IViewModelBase>();
        private readonly IContainer _iocContainer;
        private readonly IViewLocator _viewLocator;

        private readonly object _navigationLock = new object();

        private IViewModelBase? _currentViewModel;

        public DroidFrameNavigationService(
            IViewLocator viewLocator,
            IContainer iocContainer)
        {
            _viewLocator = viewLocator;
            _iocContainer = iocContainer;
        }

        /// <inheritdoc />
        public bool IsInitialized => Config != null;

        /// <inheritdoc />
        public bool IsEmptyBackStack
        {
            get
            {
                lock (_navigationLock)
                {
                    return _backStack.IsEmpty;
                }
            }
        }

        /// <inheritdoc />
        public bool CanGoBack
        {
            get
            {
                lock (_navigationLock)
                {
                    return _backStack.CanGoBack;
                }
            }
        }

        protected FrameNavigationConfig? Config { get; private set; }

        /// <inheritdoc />
        public void Initialize(object navigation)
        {
            Config = navigation as FrameNavigationConfig;
        }

        /// <inheritdoc />
        public void GoBack()
        {
            lock (_navigationLock)
            {
                if (!_backStack.CanGoBack)
                {
                    return;
                }

                // navigation
                _backStack.GoBack();

                // apply platform navigation
                ApplyBackStack(_backStack);
            }
        }

        /// <inheritdoc />
        public void GoBack<T>() where T : IViewModelBase
        {
            lock (_navigationLock)
            {
                // navigation
                _backStack.GoBackWhile(x => !(x is T));

                // apply platform navigation
                ApplyBackStack(_backStack);
            }
        }

        /// <inheritdoc />
        public virtual void NavigateToViewModel<TViewModel>(
            bool clearBackStack = false,
            IReadOnlyList<NavigationParameterModel>? parameters = null)
            where TViewModel : IViewModelBase
        {
            lock (_navigationLock)
            {
                if (clearBackStack)
                {
                    _backStack.Clear();
                }

                var viewModel = CreateViewModel<TViewModel>(parameters);
                _backStack.Add(viewModel);

                // apply platform navigation
                ApplyBackStack(_backStack);
            }
        }

        /// <inheritdoc />
        public void NavigateToFirstPage()
        {
            lock (_navigationLock)
            {
                if (_backStack.IsEmpty)
                {
                    return;
                }

                _backStack.ResetToFirst();

                // apply platform navigation
                ApplyBackStack(_backStack);
            }
        }

        /// <inheritdoc />
        public void RestoreNavigation()
        {
            lock (_navigationLock)
            {
                ApplyBackStack(_backStack);
            }
        }

        protected virtual IViewModelBase CreateViewModel<TViewModel>(IReadOnlyList<NavigationParameterModel>? parameters)
            where TViewModel : IViewModelBase
        {
            var viewModel = _iocContainer.Resolve<TViewModel>(this);

            if (parameters != null)
            {
                viewModel.ApplyParameters(parameters);
            }

            return viewModel;
        }

        protected virtual FragmentTransaction PrepareTransaction(FragmentTransaction fragmentTransaction)
        {
            return fragmentTransaction;
        }

        private static void UpdateViewModelStorage(FragmentManager fragmentManager, IViewModelBase? viewModelToRemove, IViewModelBase viewModelToAdd)
        {
            if (ReferenceEquals(viewModelToRemove, viewModelToAdd))
            {
                return;
            }

            var viewModelStore = ViewModelStore.Of(fragmentManager);

            if (viewModelToRemove != null)
            {
                RemoveViewModelFromStorage(viewModelToRemove, viewModelStore);
            }

            AddViewModelToStorage(viewModelToAdd, viewModelStore);
        }

        private static void AddViewModelToStorage(IViewModelBase viewModelToAdd, IInstanceStorage viewModelStore)
        {
            var viewModelKey = ViewModelStore.GenerateKeyForType(viewModelToAdd.GetType());
            viewModelStore.Add(viewModelKey, viewModelToAdd);
        }

        private static void RemoveViewModelFromStorage(IViewModelBase viewModelToRemove, IInstanceStorage viewModelStore)
        {
            var viewModelKey = ViewModelStore.GenerateKeyForType(viewModelToRemove.GetType());
            viewModelStore.Remove(viewModelKey);
        }

        private static object? ExtractCurrentFragmentViewModel(FrameNavigationConfig config)
        {
            var currentBindableFragment = config.Manager.FindFragmentById(config.ContainerId) as IBindable;

            return currentBindableFragment?.DataContext;
        }

        private void ApplyBackStack(BackStack<IViewModelBase> backStack)
        {
            if (Config?.Manager == null
                || Config.Manager.IsDestroyed
                || Config.Manager.IsStateSaved)
            {
                return;
            }

            var topViewModel = backStack.Current();
            UpdateViewModelStorage(Config.Manager, _currentViewModel, topViewModel);
            _currentViewModel = topViewModel;

            SetCurrentFragment(Config, topViewModel);
        }

        private void SetCurrentFragment(FrameNavigationConfig config, IViewModelBase topViewModel)
        {
            var currentFragmentViewModel = ExtractCurrentFragmentViewModel(config);
            if (ReferenceEquals(topViewModel, currentFragmentViewModel))
            {
                return;
            }

            var fragment = (Fragment) _viewLocator.GetView(topViewModel, ViewType.Fragment);

            var transaction = config.Manager
                .BeginTransaction()
                .Replace(config.ContainerId, fragment);

            var preparedTransaction = PrepareTransaction(transaction);

            Execute.BeginOnUIThread(() =>
            {
                // Commit() method is executed asynchronously, so there's no way to know when will the transaction actually complete.
                // At the same time, trying to execute multiple transactions at the same time will result in exception.
                // ExecutePendingTransactions() should prevent exceptions even if two transactions are initiated at the same time
                // IMPORTANT: ExecutePendingTransactions() is required to me called from UI thread
                config.Manager.ExecutePendingTransactions();
                preparedTransaction.Commit();
            });
        }
    }
}

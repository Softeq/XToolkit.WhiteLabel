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
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class DroidFrameNavigationService : IFrameNavigationService
    {
        private readonly BackStack<IViewModelBase> _backStack = new BackStack<IViewModelBase>();
        private readonly IContainer _iocContainer;
        private readonly IViewLocator _viewLocator;

        private IViewModelBase? _currentViewModel;

        public DroidFrameNavigationService(
            IViewLocator viewLocator,
            IContainer iocContainer)
        {
            _viewLocator = viewLocator;
            _iocContainer = iocContainer;
        }

        protected FrameNavigationConfig? Config { get; private set; }

        public bool IsInitialized => Config != null;

        public bool IsEmptyBackStack => _backStack.IsEmpty;

        public bool CanGoBack => _backStack.CanGoBack;

        public void Initialize(object navigation)
        {
            Config = navigation as FrameNavigationConfig;
        }

        public void GoBack()
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

        public void GoBack<T>() where T : IViewModelBase
        {
            // navigation
            _backStack.GoBackWhile(x => !(x is T));

            // apply platform navigation
            ApplyBackStack(_backStack);
        }

        public virtual void NavigateToViewModel<TViewModel>(
            bool clearBackStack = false,
            IReadOnlyList<NavigationParameterModel>? parameters = null)
            where TViewModel : IViewModelBase
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

        /// <inheritdoc />
        public void NavigateToFirstPage()
        {
            if (_backStack.IsEmpty)
            {
                return;
            }

            _backStack.ResetToFirst();

            // apply platform navigation
            ApplyBackStack(_backStack);
        }

        /// <inheritdoc />
        public void RestoreNavigation()
        {
            ApplyBackStack(_backStack);
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
            Execute.BeginOnUIThread(() =>
            {
                if (Config?.Manager == null
                    || Config.Manager.IsDestroyed
                    || Config.Manager.IsStateSaved)
                {
                    return;
                }

                var topViewModel = backStack.Current();
                UpdateViewModelStorage(Config.Manager, _currentViewModel, topViewModel);
                SetCurrentFragment(Config, topViewModel);
                _currentViewModel = topViewModel;
            });
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

            PrepareTransaction(transaction).Commit();
        }

        protected virtual FragmentTransaction PrepareTransaction(FragmentTransaction fragmentTransaction)
        {
            return fragmentTransaction;
        }
    }
}

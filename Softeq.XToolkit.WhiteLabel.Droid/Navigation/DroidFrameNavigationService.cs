// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Plugin.CurrentActivity;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Droid.Internal;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class DroidFrameNavigationService : IFrameNavigationService
    {
        private readonly IViewLocator _viewLocator;
        private readonly ICurrentActivity _currentActivity;
        private readonly IContainer _container;
        private readonly BackStack<(IViewModelBase ViewModel, Fragment Fragment)> _backStack;
        private readonly IViewModelStore _viewModelStore;

        private int _containerId;

        public DroidFrameNavigationService(
            IViewLocator viewLocator,
            ICurrentActivity currentActivity,
            IContainer container)
        {
            _viewLocator = viewLocator;
            _currentActivity = currentActivity;
            _container = container;

            _backStack = new BackStack<(IViewModelBase ViewModel, Fragment Fragment)>();
            _viewModelStore = ViewModelStore.Of((AppCompatActivity) _currentActivity.Activity);
        }

        public bool IsInitialized => _containerId != 0;

        public bool IsEmptyBackStack => _backStack.IsEmpty;

        public bool CanGoBack => _backStack.CanGoBack;

        public void GoBack()
        {
            // remove current
            var currentEntry = _backStack.CurrentWithRemove();

            _viewModelStore.Remove(currentEntry.Fragment.GetType().Name);

            // show previous
            RestoreState();
        }

        public void GoBack<T>() where T : IViewModelBase
        {
            throw new NotImplementedException();
        }

        public void Initialize(object navigation)
        {
            _containerId = (int) navigation;
        }

        #region remove
        public void NavigateToViewModel<T, TParameter>(TParameter parameter)
            where T : IViewModelBase, IViewModelParameter<TParameter>
        {
            throw new NotImplementedException();
        }

        public void NavigateToViewModel<T>(bool clearBackStack = false)
            where T : IViewModelBase
        {
            throw new NotImplementedException();
        }

        public void NavigateToViewModel<T>(T t)
            where T : IViewModelBase
        {
            throw new NotImplementedException();
        }
        #endregion

        // for nav from toolbarViewModel
        public void NavigateToViewModel(Type viewModelType, bool clearBackStack = false)
        {
            var viewModel = CreateViewModel(viewModelType);
            NavigateInternal(viewModel);
        }

        // for nav from frame navigation
        public void NavigateToViewModel<TViewModel>(
            IEnumerable<NavigationParameterModel> navigationParameters)
            where TViewModel : IViewModelBase
        {
            var viewModel = CreateViewModel(typeof(TViewModel));
            viewModel.ApplyParameters(navigationParameters);
            NavigateInternal(viewModel);
        }

        /// <inheritdoc />
        public void NavigateToFirstPage()
        {
            if (_backStack.IsEmpty)
            {
                return;
            }

            var entry = _backStack.ResetToFirst();

            NavigateInternal(entry.ViewModel);
        }

        /// <inheritdoc />
        public void RestoreState()
        {
            var entry = _backStack.Current();

            ReplaceFragment(entry.Fragment);
        }

        private void NavigateInternal(IViewModelBase viewModel)
        {
            var fragment = (Fragment) _viewLocator.GetView(viewModel, ViewType.Fragment);

            _backStack.Add((viewModel, fragment));

            _viewModelStore.Add(fragment.GetType().Name, viewModel);

            ReplaceFragment(fragment);
        }

        protected virtual void ReplaceFragment(Fragment fragment)
        {
            Execute.BeginOnUIThread(() =>
            {
                var activity = (AppCompatActivity) _currentActivity.Activity;
                var fragmentManager = activity.SupportFragmentManager;

                var transaction = fragmentManager
                    .BeginTransaction()
                    .Replace(_containerId, fragment);

                PrepareTransaction(transaction).Commit();
            });
        }

        protected virtual FragmentTransaction PrepareTransaction(FragmentTransaction fragmentTransaction)
        {
            return fragmentTransaction;
        }

        protected virtual IViewModelBase CreateViewModel(Type viewModelType)
        {
            if (!typeof(IViewModelBase).IsAssignableFrom(viewModelType))
            {
                throw new ArgumentException($"Class must implement {nameof(IViewModelBase)}");
            }

            var viewModel = (IViewModelBase) _container.Resolve(viewModelType);

            if (viewModel is IFrameViewModel frameViewModel)
            {
                frameViewModel.FrameNavigationService = this;
            }

            return viewModel;
        }
    }
}

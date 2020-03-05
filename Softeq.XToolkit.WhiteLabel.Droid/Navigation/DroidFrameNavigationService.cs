// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Plugin.CurrentActivity;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Droid.Internal;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class DroidFrameNavigationService : IFrameNavigationService
    {
        private readonly BackStack<(IViewModelBase ViewModel, Fragment Fragment)> _backStack;
        private readonly ICurrentActivity _currentActivity;
        private readonly IContainer _iocContainer;
        private readonly IViewLocator _viewLocator;
        private readonly IViewModelStore _viewModelStore;

        private int _containerId;

        public DroidFrameNavigationService(
            IViewLocator viewLocator,
            ICurrentActivity currentActivity,
            IContainer iocContainer)
        {
            _viewLocator = viewLocator;
            _currentActivity = currentActivity;
            _iocContainer = iocContainer;

            _backStack = new BackStack<(IViewModelBase ViewModel, Fragment Fragment)>();
            _viewModelStore = ViewModelStore.Of((AppCompatActivity) _currentActivity.Activity);
        }

        public bool IsInitialized => _containerId != 0;

        public bool IsEmptyBackStack => _backStack.IsEmpty;

        public bool CanGoBack => _backStack.CanGoBack;

        public void Initialize(object navigation)
        {
            _containerId = (int) navigation;
        }

        public void GoBack()
        {
            // navigation
            var currentEntry = _backStack.CurrentWithRemove();
            var currentFrameName = ToKey(currentEntry.Fragment);

            // cleanup
            _viewModelStore.Remove(currentFrameName);

            // show
            RestoreNavigation();
        }

        public void GoBack<T>() where T : IViewModelBase
        {
            // navigation
            var dumpBefore = _backStack.Dump(x => ToKey(x.Fragment));

            _backStack.GoBackWhile(x => !(x.ViewModel is T));

            var dumpAfter = _backStack.Dump(x => ToKey(x.Fragment));

            var fragmentNamesForRemove = dumpBefore.Except(dumpAfter).ToArray();

            // cleanup
            _viewModelStore.Remove(fragmentNamesForRemove);

            // show
            RestoreNavigation();
        }

        public virtual void NavigateToViewModel<TViewModel>(
            bool clearBackStack = false,
            IReadOnlyList<NavigationParameterModel>? parameters = null)
            where TViewModel : IViewModelBase
        {
            var viewModel = CreateViewModel<TViewModel>(parameters);

            if (clearBackStack && !IsEmptyBackStack)
            {
                ClearBackStack();
            }

            NavigateInternal(viewModel);
        }

        /// <inheritdoc />
        public void NavigateToFirstPage()
        {
            if (IsEmptyBackStack)
            {
                return;
            }

            var entry = _backStack.ResetToFirst();

            NavigateInternal(entry.ViewModel);
        }

        /// <inheritdoc />
        public void RestoreNavigation()
        {
            var entry = _backStack.Current();

            ReplaceFragment(entry.Fragment);
        }

        private void NavigateInternal(IViewModelBase viewModel)
        {
            var fragment = (Fragment) _viewLocator.GetView(viewModel, ViewType.Fragment);

            _backStack.Add((viewModel, fragment));

            _viewModelStore.Add(ToKey(fragment), viewModel);

            ReplaceFragment(fragment);
        }

        private void ClearBackStack()
        {
            var fragmentNames = _backStack.Dump(x => ToKey(x.Fragment));

            _backStack.Clear();

            _viewModelStore.Remove(fragmentNames);
        }

        private string ToKey(Fragment fragment)
        {
            return fragment.GetType().Name;
        }

        protected virtual IViewModelBase CreateViewModel<TViewModel>(IReadOnlyList<NavigationParameterModel>? parameters)
            where TViewModel : notnull
        {
            if (!typeof(IViewModelBase).IsAssignableFrom(typeof(TViewModel)))
            {
                throw new ArgumentException($"Class must implement {nameof(IViewModelBase)}");
            }

            var viewModel = (IViewModelBase) _iocContainer.Resolve<TViewModel>(this);

            if (parameters != null)
            {
                viewModel.ApplyParameters(parameters);
            }

            return viewModel;
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
    }
}
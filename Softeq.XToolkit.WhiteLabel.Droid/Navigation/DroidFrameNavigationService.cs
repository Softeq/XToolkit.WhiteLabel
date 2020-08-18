// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
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
        private readonly IContainer _iocContainer;
        private readonly IViewLocator _viewLocator;
        private readonly IViewModelStore _viewModelStore;

        private FrameNavigationConfig? _config;

        public DroidFrameNavigationService(
            IViewLocator viewLocator,
            ICurrentActivity currentActivity,
            IContainer iocContainer)
        {
            _viewLocator = viewLocator;
            _iocContainer = iocContainer;

            _backStack = new BackStack<(IViewModelBase ViewModel, Fragment Fragment)>();
            _viewModelStore = ViewModelStore.Of((AppCompatActivity) currentActivity.Activity);
        }

        public bool IsInitialized => _config != null;

        public bool IsEmptyBackStack => _backStack.IsEmpty;

        public bool CanGoBack => _backStack.CanGoBack;

        public void Initialize(object navigation)
        {
            _config = navigation as FrameNavigationConfig;
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

        private static string ToKey(object fragment)
        {
            return fragment.GetType().Name;
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
                if (_config == null)
                {
                    return;
                }

                var transaction = _config.Manager
                    .BeginTransaction()
                    .Replace(_config.ContainerId, fragment);

                PrepareTransaction(transaction).Commit();
            });
        }

        protected virtual FragmentTransaction PrepareTransaction(FragmentTransaction fragmentTransaction)
        {
            return fragmentTransaction;
        }
    }
}

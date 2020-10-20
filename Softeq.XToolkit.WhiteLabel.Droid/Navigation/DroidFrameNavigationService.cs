// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly BackStack<Fragment> _backStack = new BackStack<Fragment>();
        private readonly IContainer _iocContainer;
        private readonly IViewLocator _viewLocator;

        private FrameNavigationConfig? _config;
        private bool _hasUnfinishedNavigation;

        public DroidFrameNavigationService(
            IViewLocator viewLocator,
            IContainer iocContainer)
        {
            _viewLocator = viewLocator;
            _iocContainer = iocContainer;
        }

        private IViewModelStore CurrentStore
        {
            get
            {
                if (_config == null)
                {
                    throw new InvalidOperationException("Navigation not initialized");
                }

                return ViewModelStore.Of(_config.Manager);
            }
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
            var currentFrameName = ToKey(currentEntry);

            // cleanup
            CurrentStore.Remove(currentFrameName);

            // show
            RestoreNavigation();
        }

        public void GoBack<T>() where T : IViewModelBase
        {
            // navigation
            var dumpBefore = _backStack.Dump(ToKey);

            _backStack.GoBackWhile(x => !(ExtractViewModel(x) is T));

            var dumpAfter = _backStack.Dump(ToKey);

            var fragmentNamesForRemove = dumpBefore.Except(dumpAfter).ToArray();

            // cleanup
            CurrentStore.Remove(fragmentNamesForRemove);

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

            var fragment = (Fragment) _viewLocator.GetView(viewModel, ViewType.Fragment);

            NavigateInternal(fragment, viewModel);
        }

        /// <inheritdoc />
        public void NavigateToFirstPage()
        {
            if (IsEmptyBackStack)
            {
                return;
            }

            var fragment = _backStack.ResetToFirst();
            var viewModel = ExtractViewModel(fragment);

            NavigateInternal(fragment, viewModel);
        }

        /// <inheritdoc />
        public void RestoreNavigation()
        {
            ReplaceFragment(_backStack.Current());
        }

        /// <inheritdoc />
        public void RestoreUnfinishedNavigation()
        {
            if (_hasUnfinishedNavigation)
            {
                RestoreNavigation();
            }
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
                if (_config?.Manager == null
                    || _config.Manager.IsDestroyed
                    || _config.Manager.IsStateSaved)
                {
                    _hasUnfinishedNavigation = true;
                    return;
                }

                _hasUnfinishedNavigation = false;
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

        protected virtual string ToKey(Fragment fragment)
        {
            return fragment.GetType().Name;
        }

        private static IViewModelBase ExtractViewModel(Fragment fragment)
        {
            return (IViewModelBase) ((IBindable) fragment).DataContext;
        }

        private void NavigateInternal(Fragment fragment, IViewModelBase viewModel)
        {
            _backStack.Add(fragment);
            CurrentStore.Add(ToKey(fragment), viewModel);
            ReplaceFragment(fragment);
        }

        private void ClearBackStack()
        {
            var fragmentNames = _backStack.Dump(ToKey);

            _backStack.Clear();

            CurrentStore.Remove(fragmentNames);
        }
    }
}

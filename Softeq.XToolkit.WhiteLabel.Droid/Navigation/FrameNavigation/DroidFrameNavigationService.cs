// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using AndroidX.Fragment.App;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Droid.Internal;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation.FrameNavigation
{
    public class DroidFrameNavigationService : IFrameNavigationService
    {
        private readonly BackStack<FragmentState> _backStack = new BackStack<FragmentState>();
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
            if (IsEmptyBackStack)
            {
                return;
            }

            var canGoBack = CanGoBack;

            // navigation
            var currentEntry = _backStack.CurrentWithRemove();
            var currentFrameName = ToKey(currentEntry);

            // cleanup
            CurrentStore.Remove(currentFrameName);

            // show previous if needed
            if (canGoBack)
            {
                RestoreNavigation();
            }
        }

        public void GoBack<T>() where T : IViewModelBase
        {
            // navigation
            var dumpBefore = _backStack.Dump(ToKey);

            _backStack.GoBackWhile(x => x.IsViewModelOfType<T>());

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

            var dumpBefore = _backStack.Dump(ToKey);

            _backStack.ResetToFirst();

            var dumpAfter = _backStack.Dump(ToKey);

            var fragmentNamesForRemove = dumpBefore.Except(dumpAfter).ToArray();

            CurrentStore.Remove(fragmentNamesForRemove);

            RestoreNavigation();
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

        protected virtual void ReplaceFragment(FragmentState fragmentState)
        {
            Execute.BeginOnUIThread(() =>
            {
                if (_config == null || !_config.CanReplaceFragment)
                {
                    _hasUnfinishedNavigation = true;
                    return;
                }

                _hasUnfinishedNavigation = false;

                PrepareTransaction(_config.ReplaceFragment(fragmentState))
                    .Commit();
            });
        }

        protected virtual FragmentTransaction PrepareTransaction(FragmentTransaction fragmentTransaction)
        {
            return fragmentTransaction;
        }

        protected virtual string ToKey(FragmentState fragmentState)
        {
            return fragmentState.Key;
        }

        private void NavigateInternal(Fragment fragment, IViewModelBase viewModel)
        {
            var fragmentState = new FragmentState(fragment, viewModel.GetType());

            _backStack.Add(fragmentState);
            CurrentStore.Add(ToKey(fragmentState), viewModel);
            ReplaceFragment(fragmentState);
        }

        private void ClearBackStack()
        {
            var fragmentNames = _backStack.Dump(ToKey);

            _backStack.Clear();

            CurrentStore.Remove(fragmentNames);
        }
    }
}

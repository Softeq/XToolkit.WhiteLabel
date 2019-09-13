// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Droid.ViewComponents;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    public abstract class ActivityBase : AppCompatActivity
    {
        private readonly IPageNavigationService _pageNavigation;

        protected ActivityBase()
        {
            _pageNavigation = Dependencies.PageNavigationService;
            ViewComponents = new List<IViewComponent<ActivityBase>>();
        }

        public List<IViewComponent<ActivityBase>> ViewComponents { get; }

        public override void OnBackPressed()
        {
            if (_pageNavigation.CanGoBack)
            {
                _pageNavigation.GoBack();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            Dependencies.PermissionRequestHandler?.Handle(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public abstract class ActivityBase<TViewModel> : ActivityBase, IBindingsOwner
        where TViewModel : ViewModelBase
    {
        private Lazy<TViewModel> _viewModelLazy;
        private readonly IBundleService _bundleService;

        protected ActivityBase()
        {
            Bindings = new List<Binding>();

            _viewModelLazy = new Lazy<TViewModel>(() =>
            {
                var backStack = Dependencies.Container.Resolve<IBackStackManager>();
                return backStack.GetExistingOrCreateViewModel<TViewModel>();
            });
            _bundleService = Dependencies.Container.Resolve<IBundleService>();
        }

        public List<Binding> Bindings { get; }

        protected virtual TViewModel ViewModel
        {
            get
            {
                if (Handle == IntPtr.Zero)
                {
                    throw new InvalidOperationException("Don't forget to detach last ViewModel bindings.");
                }

                return _viewModelLazy.Value;
            }
        }

        protected virtual ScreenOrientation DefaultScreenOrientation { get; } = ScreenOrientation.Portrait;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestedOrientation = DefaultScreenOrientation;

            _bundleService.TryToRestoreParams(ViewModel, Intent, savedInstanceState);

            if (!ViewModel.IsInitialized)
            {
                ViewModel.OnInitialize();
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            ViewModel.OnAppearing();
            DoAttachBindings();
        }

        protected override void OnPause()
        {
            base.OnPause();

            DoDetachBindings();
            ViewModel.OnDisappearing();
        }

        protected override void OnDestroy()
        {
            if (IsFinishing)
            {
                _viewModelLazy = null;
            }
            base.OnDestroy();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            _bundleService.SaveInstanceState(outState);

            base.OnSaveInstanceState(outState);
        }

        protected virtual void DoAttachBindings()
        {
        }

        protected virtual void DoDetachBindings()
        {
            this.DetachBindings();
        }
    }

    public abstract class ActivityBase<TViewModel, TInterface> : ActivityBase<TViewModel>
        where TViewModel : ViewModelBase, TInterface
        where TInterface : IViewModelBase
    {
        private TViewModel _viewModel;

        protected override TViewModel ViewModel =>
            _viewModel ?? (_viewModel = (TViewModel) Dependencies.Container.Resolve<TInterface>());
    }
}

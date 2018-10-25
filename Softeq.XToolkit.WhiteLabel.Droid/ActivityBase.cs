// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Permission = Android.Content.PM.Permission;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    public abstract class ActivityBase : AppCompatActivity
    {
        public override void OnBackPressed()
        {
            var pageNavigation = ServiceLocator.Resolve<IPageNavigationService>();
            if (pageNavigation.CanGoBack)
            {
                pageNavigation.GoBack();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        protected void AddViewForViewModel(ViewModelBase viewModel, int containerId)
        {
            var viewLocator = ServiceLocator.Resolve<ViewLocator>();
            var fragment = (Fragment) viewLocator.GetView(viewModel, ViewType.Fragment);
            SupportFragmentManager
                .BeginTransaction()
                .Add(containerId, fragment)
                .Commit();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (ServiceLocator.IsRegistered<IPermissionRequestHandler>())
            {
                var permissionRequestHandler = ServiceLocator.Resolve<IPermissionRequestHandler>();
                permissionRequestHandler.Handle(requestCode, permissions, grantResults);
            }

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public abstract class ActivityBase<TViewModel> : ActivityBase
        where TViewModel : ViewModelBase
    {
        protected TViewModel _viewModel;
        protected List<Binding> Bindings { get; } = new List<Binding>();

        public virtual TViewModel ViewModel => _viewModel ?? (_viewModel = ServiceLocator.Resolve<TViewModel>());

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(null);

            RequestedOrientation = ScreenOrientation.Portrait;

#if DEBUG
            var vmPolicy = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(vmPolicy.DetectActivityLeaks().PenaltyLog().Build());
#endif

            ViewModel.OnInitialize();
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
            base.OnDestroy();

            _viewModel = null;
            Dispose();
        }

        protected virtual void DoAttachBindings()
        {
        }

        protected virtual void DoDetachBindings()
        {
            Bindings.DetachAllAndClear();
        }
    }
    
    public abstract class ActivityBase<TViewModel, TInterface> : ActivityBase<TViewModel>
        where TViewModel : ViewModelBase, TInterface
        where TInterface : IViewModelBase
    {
        public override TViewModel ViewModel =>
            _viewModel ?? (_viewModel = (TViewModel) ServiceLocator.Resolve<TInterface>());
    }
}
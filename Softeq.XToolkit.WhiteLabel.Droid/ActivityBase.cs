// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Newtonsoft.Json.Linq;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Droid.ViewComponents;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Permission = Android.Content.PM.Permission;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    public abstract class ActivityBase : AppCompatActivity
    {
        private readonly Lazy<IPageNavigationService> _pageNavigationLazy;

        protected ActivityBase()
        {
            _pageNavigationLazy = new Lazy<IPageNavigationService>(ServiceLocator.Resolve<IPageNavigationService>);
        }

        public List<IViewComponent<ActivityBase>> ViewComponents { get; private set; }

        public override void OnBackPressed()
        {
            if (_pageNavigationLazy.Value.CanGoBack)
            {
                _pageNavigationLazy.Value.GoBack();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            if (ServiceLocator.IsRegistered<IPermissionRequestHandler>())
            {
                var permissionRequestHandler = ServiceLocator.Resolve<IPermissionRequestHandler>();
                permissionRequestHandler.Handle(requestCode, permissions, grantResults);
            }

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ViewComponents = new List<IViewComponent<ActivityBase>>();
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
    }

    public abstract class ActivityBase<TViewModel> : ActivityBase
        where TViewModel : ViewModelBase
    {
        private const string ShouldRestoreStateKey = "shouldRestore";
        private readonly Lazy<IJsonSerializer> _jsonSerializerLazy;
        private Lazy<TViewModel> _viewModel;

        protected ActivityBase()
        {
            Bindings = new List<Binding>();
            _jsonSerializerLazy = new Lazy<IJsonSerializer>(ServiceLocator.Resolve<IJsonSerializer>);
            _viewModel = new Lazy<TViewModel>(() => ServiceLocator.Resolve<IBackStackManager>()
                .GetExistingOrCreateViewModel<TViewModel>());
        }

        protected List<Binding> Bindings { get; }
        protected virtual TViewModel ViewModel => _viewModel.Value;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(null);

            RequestedOrientation = ScreenOrientation.Portrait;
#if DEBUG
            var vmPolicy = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(vmPolicy.DetectActivityLeaks().PenaltyLog().Build());
#endif
            RestoreIfNeeded(savedInstanceState);

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

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt(ShouldRestoreStateKey, 0);
            base.OnSaveInstanceState(outState);
        }

        protected override void OnDestroy()
        {
            if (IsFinishing)
            {
                _viewModel = null;
                base.OnDestroy();
                Dispose();
            }
            else
            {
                base.OnDestroy();
            }
        }

        protected virtual void DoAttachBindings()
        {
        }

        protected virtual void DoDetachBindings()
        {
            Bindings.DetachAllAndClear();
        }

        private void RestoreIfNeeded(Bundle outState)
        {
            /* We skip restore if:
                1) viewmodel was alive
                2) activity never been destroyed
                3) we don't have data to restore
            */
            if (ViewModel.IsInitialized || outState == null || !outState.ContainsKey(ShouldRestoreStateKey) ||
                !Intent.HasExtra(Constants.ParametersKey))
            {
                return;
            }

            var parametersObject = Intent.GetStringExtra(Constants.ParametersKey);
            var parameters =
                _jsonSerializerLazy.Value.Deserialize<IReadOnlyList<NavigationParameterModel>>(parametersObject);

            foreach (var parameter in parameters)
            {
                SetValueToProperty(parameter);
            }

            Intent.RemoveExtra(ShouldRestoreStateKey);
        }

        private void SetValueToProperty(NavigationParameterModel parameter)
        {
            var property = parameter.PropertyInfo.ToProperty();

            object GetValue(object value)
            {
                if (property.PropertyType.IsEnum)
                {
                    return Enum.ToObject(property.PropertyType, value);
                }

                return ((JObject) value).ToObject(property.PropertyType);
            }

            property.SetValue(ViewModel, GetValue(parameter.Value), null);
        }
    }

    public abstract class ActivityBase<TViewModel, TInterface> : ActivityBase<TViewModel>
        where TViewModel : ViewModelBase, TInterface
        where TInterface : IViewModelBase
    {
        private TViewModel _viewModel;

        protected override TViewModel ViewModel =>
            _viewModel ?? (_viewModel = (TViewModel) ServiceLocator.Resolve<TInterface>());
    }
}
﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
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
        protected readonly Lazy<IPageNavigationService> PageNavigation;
        public List<IViewComponent<ActivityBase>> ViewComponents { get; private set; }

        protected ActivityBase()
        {
            PageNavigation = new Lazy<IPageNavigationService>(ServiceLocator.Resolve<IPageNavigationService>);
        }

        public override void OnBackPressed()
        {
            if (PageNavigation.Value.CanGoBack)
            {
                PageNavigation.Value.GoBack();
            }

            base.OnBackPressed();
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
        private readonly Lazy<IJsonSerializer> _jsonSerializer;
        protected List<Binding> Bindings { get; }
        protected virtual TViewModel ViewModel { get; set; }

        protected ActivityBase()
        {
            Bindings = new List<Binding>();
            _jsonSerializer = new Lazy<IJsonSerializer>(ServiceLocator.Resolve<IJsonSerializer>);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(null);

            if (ViewModel == null)
            {
                ViewModel = (TViewModel) PageNavigation.Value.GetExistingOrCreateViewModel(typeof(TViewModel));
            }

            RequestedOrientation = ScreenOrientation.Portrait;
#if DEBUG
            var vmPolicy = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(vmPolicy.DetectActivityLeaks().PenaltyLog().Build());
#endif
            RestoreIfNeeded();

            ViewModel.OnInitialize();
        }

        private void RestoreIfNeeded()
        {
            if (ViewModel.IsInitialized || Intent.HasExtra(ShouldRestoreStateKey) ||
                !Intent.HasExtra(Constants.ParametersKey))
            {
                return;
            }

            var parametersObject = Intent.GetStringExtra(Constants.ParametersKey);
            var parameters =
                _jsonSerializer.Value.Deserialize<IReadOnlyCollection<NavigationParameterModel>>(parametersObject);

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
            Intent.PutExtra(ShouldRestoreStateKey, true);

            if (IsFinishing)
            {
                ViewModel = null;
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
    }

    public abstract class ActivityBase<TViewModel, TInterface> : ActivityBase<TViewModel>
        where TViewModel : ViewModelBase, TInterface
        where TInterface : IViewModelBase
    {
        protected override TViewModel ViewModel =>
            ViewModel ?? (ViewModel = (TViewModel) ServiceLocator.Resolve<TInterface>());
    }
}
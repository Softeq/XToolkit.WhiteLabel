// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Newtonsoft.Json.Linq;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Interfaces;
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

    public abstract class ActivityBase<TViewModel> : ActivityBase, IBindableOwner
        where TViewModel : ViewModelBase
    {
        private const string ShouldRestoreStateKey = "shouldRestore";
        private readonly IJsonSerializer _jsonSerializer;
        private Lazy<TViewModel> _viewModelLazy;

        protected ActivityBase()
        {
            Bindings = new List<Binding>();
            _jsonSerializer = Dependencies.JsonSerializer;
            _viewModelLazy = new Lazy<TViewModel>(() =>
            {
                var backStack = Dependencies.Container.Resolve<IBackStackManager>();
                return backStack.GetExistingOrCreateViewModel<TViewModel>();
            });
        }

        protected virtual TViewModel ViewModel
        {
            get
            {
                if (Handle == IntPtr.Zero)
                {
                    throw new Exception("Don't forget to detach last ViewModel bindings.");
                }

                return _viewModelLazy.Value;
            }
        }

        public List<Binding> Bindings { get; }

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
                _viewModelLazy = null;
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
                _jsonSerializer.Deserialize<IReadOnlyList<NavigationParameterModel>>(parametersObject);

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
            _viewModel ?? (_viewModel = (TViewModel) Dependencies.Container.Resolve<TInterface>());
    }
}

﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Droid.Permissions;
using Softeq.XToolkit.WhiteLabel.Droid.Interfaces;
using Softeq.XToolkit.WhiteLabel.Droid.ViewComponents;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    /// <summary>
    ///     Based on <see cref="T:AndroidX.AppCompat.App.AppCompatActivity"/>, used for creating Activities.
    /// </summary>
    public abstract class ActivityBase : AppCompatActivity
    {
        private readonly IPageNavigationService _pageNavigation;

        protected ActivityBase()
        {
            _pageNavigation = Dependencies.PageNavigationService;
            ViewComponents = new List<IViewComponent<ActivityBase>>();
        }

        // ReSharper disable once CollectionNeverUpdated.Global
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

        public override void OnRequestPermissionsResult(
            int requestCode,
            string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            Dependencies.Container.Resolve<IPermissionRequestHandler>()?.Handle(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    /// <summary>
    ///     Generic class based on <see cref="T:AndroidX.AppCompat.App.AppCompatActivity"/>, used for creating Activities.
    /// </summary>
    /// <typeparam name="TViewModel">Type of ViewModel.</typeparam>
    public abstract class ActivityBase<TViewModel> : ActivityBase, IBindingsOwner
        where TViewModel : ViewModelBase
    {
        private readonly IBundleService _bundleService;
        private Lazy<TViewModel>? _viewModelLazy;

        protected ActivityBase()
        {
            Bindings = new List<Binding>();

            _viewModelLazy = new Lazy<TViewModel>(() =>
            {
                var backStack = Dependencies.Container.Resolve<IBackStackManager>();
                if (backStack.Count > 0 && backStack.PeekViewModel() is TViewModel viewModel)
                {
                    return viewModel;
                }
                else
                {
                    viewModel = Dependencies.Container.Resolve<TViewModel>();
                    backStack.PushViewModel(viewModel);
                    return viewModel;
                }
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

                if (_viewModelLazy == null)
                {
                    throw new InvalidOperationException("Activity has been destroyed");
                }

                return _viewModelLazy.Value;
            }
        }

        protected virtual ScreenOrientation DefaultScreenOrientation { get; } = ScreenOrientation.Portrait;

        protected override void OnCreate(Bundle? savedInstanceState)
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
        private TViewModel? _viewModel;

        protected override TViewModel ViewModel => _viewModel ??= (TViewModel) Dependencies.Container.Resolve<TInterface>();
    }
}

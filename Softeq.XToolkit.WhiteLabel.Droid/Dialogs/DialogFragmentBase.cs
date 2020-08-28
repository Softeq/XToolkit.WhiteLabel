﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    public abstract class DialogFragmentBase<TViewModel> : DialogFragment, IBindable
        where TViewModel : DialogViewModelBase
    {
        protected TViewModel ViewModel => (TViewModel) DataContext;

        protected virtual int ThemeId { get; } = Resource.Style.CoreDialogTheme;
        public List<Binding> Bindings { get; } = new List<Binding>();

        public object DataContext { get; private set; } = default!;

        void IBindable.SetDataContext(object dataContext)
        {
            DataContext = dataContext;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RestoreViewModelIfNeeded(savedInstanceState);

            OnViewModelRestored();

            if (!ViewModel.IsInitialized)
            {
                ViewModel.OnInitialize();
            }
        }

        public override void OnResume()
        {
            base.OnResume();

            ViewModel.OnAppearing();
            DoAttachBindings();
        }

        public override void OnPause()
        {
            base.OnPause();

            DoDetachBindings();
            ViewModel.OnDisappearing();
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);

            ViewModel?.DialogComponent.CloseCommand.Execute(null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            ViewModel.DialogComponent.Closed += DialogComponentOnClosed;
        }

        public override void OnDestroyView()
        {
            ViewModel.DialogComponent.Closed -= DialogComponentOnClosed;
            DataContext = null;

            base.OnDestroyView();
        }

        public void Show()
        {
            SetStyle(StyleNoFrame, ThemeId);

            var contextProvider = Dependencies.Container.Resolve<IContextProvider>();
            var baseActivity = (FragmentActivity) contextProvider.CurrentActivity;

            Internal.ViewModelStore.Of(baseActivity).Add(GetKey(), ViewModel);
            Show(baseActivity.SupportFragmentManager, null);
        }

        protected virtual void RestoreViewModelIfNeeded(Bundle? savedInstanceState)
        {
            if (ViewModel == null && savedInstanceState != null)
            {
                var viewModelStore = Internal.ViewModelStore.Of(Activity);
                DataContext = (TViewModel) viewModelStore.Get<IDialogViewModel>(GetKey());
            }
        }

        protected virtual void DoAttachBindings()
        {
        }

        protected virtual void DoDetachBindings()
        {
            this.DetachBindings();
        }

        protected virtual void OnViewModelRestored()
        {
        }

        private void DialogComponentOnClosed(object sender, EventArgs e)
        {
            Dismiss();

            var contextProvider = Dependencies.Container.Resolve<IContextProvider>();
            var baseActivity = (FragmentActivity) contextProvider.CurrentActivity;
            Internal.ViewModelStore.Of(baseActivity).Remove(GetKey());
        }

        private string GetKey()
        {
            return GetType().Name;
        }
    }
}

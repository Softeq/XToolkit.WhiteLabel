// Developed by Softeq Development Corporation
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
using Softeq.XToolkit.WhiteLabel.Navigation;
using Dialog = Android.App.Dialog;

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    public abstract class DialogFragmentBase<TViewModel> : DialogFragment, IBindable, IDialogFragment
        where TViewModel : class, IDialogViewModel
    {
        private const string ViewModelKey = "ViewModelKey";

        private readonly Lazy<IContextProvider> _contextProviderLazy = Dependencies.Container.Resolve<Lazy<IContextProvider>>();

        private string? _key;

        public event EventHandler? WillDismiss;

        public event EventHandler? Dismissed;

        protected TViewModel ViewModel => (TViewModel) DataContext;

        protected virtual int ThemeId { get; } = Resource.Style.CoreDialogTheme;

        protected virtual int? DialogAnimationId { get; }

        public List<Binding> Bindings { get; } = new List<Binding>();

        public object DataContext { get; private set; } = default!;

        void IBindable.SetDataContext(object dataContext)
        {
            DataContext = dataContext;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (savedInstanceState != null)
            {
                _key = savedInstanceState.GetString(ViewModelKey);
            }

            RestoreViewModelIfNeeded(savedInstanceState);

            OnViewModelRestored();

            if (!ViewModel.IsInitialized)
            {
                ViewModel.OnInitialize();
            }
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var dialog = base.OnCreateDialog(savedInstanceState);

            SetupDialogAnimation(dialog);

            return dialog;
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

        public override void Dismiss()
        {
            WillDismiss?.Invoke(this, null);

            if (Activity != null && !Activity.IsFinishing && !Activity.IsDestroyed)
            {
                base.Dismiss();
            }
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);

            Dismissed?.Invoke(this, null);

            ViewModel.DialogComponent.CloseCommand.Execute(null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            ViewModel.DialogComponent.Closed += DialogComponentOnClosed;
        }

        public override void OnDestroyView()
        {
            ViewModel.DialogComponent.Closed -= DialogComponentOnClosed;

            base.OnDestroyView();
        }

        public void Show()
        {
            SetStyle(StyleNoFrame, ThemeId);

            var fragmentManager = GetFragmentManager();

            Internal.ViewModelStore.Of(fragmentManager).Add(GetKey(), ViewModel);
            Show(fragmentManager, null);
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutString(ViewModelKey, _key);
        }

        protected void SetupDialogAnimation(Dialog dialog)
        {
            if (DialogAnimationId.HasValue && dialog?.Window?.Attributes != null)
            {
                dialog.Window.Attributes.WindowAnimations = DialogAnimationId.Value;
            }
        }

        protected virtual void RestoreViewModelIfNeeded(Bundle? savedInstanceState)
        {
            if (ViewModel == null && savedInstanceState != null)
            {
                var fragmentManager = GetFragmentManager();
                var viewModelStore = Internal.ViewModelStore.Of(fragmentManager);
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

            var fragmentManager = GetFragmentManager();
            if (!fragmentManager.IsDestroyed)
            {
                Internal.ViewModelStore.Of(fragmentManager).Remove(GetKey());
            }
        }

        private string GetKey()
        {
            _key ??= Guid.NewGuid().ToString();

            return _key;
        }

        private FragmentManager GetFragmentManager()
        {
            var contextProvider = _contextProviderLazy.Value;
            var fragmentActivity = (FragmentActivity) contextProvider.CurrentActivity;
            return fragmentActivity.SupportFragmentManager;
        }
    }
}

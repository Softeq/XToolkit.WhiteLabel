// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Plugin.CurrentActivity;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    public abstract class DialogFragmentBase<TViewModel> : DialogFragment, IBindableOwner
        where TViewModel : IDialogViewModel
    {
        public List<Binding> Bindings { get; } = new List<Binding>();

        public TViewModel ViewModel { get; private set; }

        protected virtual int ThemeId { get; } = Resource.Style.CoreDialogTheme;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.OnInitialize();
            ViewModel.DialogComponent.SetCommand(nameof(ViewModel.DialogComponent.Closed), new RelayCommand(Dismiss));
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

            ViewModel.DialogComponent.CloseCommand.Execute(null);
        }

        public void SetExistingViewModel(TViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public void Show()
        {
            SetStyle(StyleNoFrame, ThemeId);
            var baseActivity = (ActivityBase) CrossCurrentActivity.Current.Activity;
            Show(baseActivity.SupportFragmentManager, null);
        }

        protected virtual void DoAttachBindings()
        {
        }

        protected virtual void DoDetachBindings()
        {
            Bindings.DetachAllAndClear();
        }
    }
}
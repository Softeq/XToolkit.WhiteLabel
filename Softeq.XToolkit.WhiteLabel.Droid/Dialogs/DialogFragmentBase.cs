﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Plugin.CurrentActivity;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    public abstract class DialogFragmentBase<TViewModel> : DialogFragment
        where TViewModel : IDialogViewModel
    {
        protected IList<Binding> Bindings { get; } = new List<Binding>();

        public TViewModel ViewModel { get; private set; }

        protected virtual int ThemeId { get; } = Resource.Style.CoreDialogTheme;

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

        protected void AddViewForViewModel(ViewModelBase viewModel, int containerId)
        {
            var viewLocator = Dependencies.Container.Resolve<IViewLocator>();
            var fragment = (Fragment) viewLocator.GetView(viewModel, ViewType.Fragment);
            ChildFragmentManager
                .BeginTransaction()
                .Add(containerId, fragment)
                .Commit();
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
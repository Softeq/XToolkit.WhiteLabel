﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.App;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    public class FragmentBase<TViewModel> : Fragment
        where TViewModel : ViewModelBase
    {
        protected IList<Binding> Bindings { get; } = new List<Binding>();

        public TViewModel ViewModel { get; private set; }

        public void SetExistingViewModel(TViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.OnInitialize();
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

        public override void OnDestroy()
        {
            base.OnDestroy();

            Dispose();
        }
        
        protected void AddViewForViewModel(ViewModelBase viewModel, int containerId)
        {
            var viewLocator = Dependencies.IocContainer.Resolve<ViewLocator>();
            var fragment = (Fragment)viewLocator.GetView(viewModel, ViewType.Fragment);
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
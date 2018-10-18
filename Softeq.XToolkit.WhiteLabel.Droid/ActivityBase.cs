// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Android.OS;
using Android.Support.V7.App;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    public class ActivityBase : AppCompatActivity
    {
        protected void AddViewForViewModel(ViewModelBase viewModel, int containerId)
        {
            var viewLocator = ServiceLocator.Resolve<ViewLocator>();
            var fragment = (Android.Support.V4.App.Fragment) viewLocator.GetView(viewModel, ViewType.Fragment);
            SupportFragmentManager
                .BeginTransaction()
                .Add(containerId, fragment)
                .Commit();
        }
    }

    public class ActivityBase<TViewModel> : ActivityBase
        where TViewModel : ViewModelBase
    {
        private TViewModel _viewModel;
        protected IList<Binding> Bindings { get; } = new List<Binding>();

        public TViewModel ViewModel => _viewModel ?? (_viewModel = ServiceLocator.Resolve<TViewModel>());

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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(null);

            ViewModel.OnInitialize();
        }

        protected override void OnResume()
        {
            base.OnResume();

            ViewModel.OnAppearing();
            AttachBindings();
        }

        protected override void OnPause()
        {
            base.OnPause();

            DetachBindings();
            ViewModel.OnDisappearing();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Dispose();
        }

        protected virtual void DoAttachBindings()
        {
        }

        protected virtual void DoDetachBindings()
        {
            Bindings.DetachAllAndClear();
        }

        private void AttachBindings()
        {
            DoAttachBindings();
        }

        private void DetachBindings()
        {
            DoDetachBindings();
        }
    }
}
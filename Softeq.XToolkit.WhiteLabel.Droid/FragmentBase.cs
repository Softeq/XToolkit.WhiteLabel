// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Android.OS;
using AndroidX.Fragment.App;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    public class FragmentBase<TViewModel> : Fragment, IBindable
        where TViewModel : ViewModelBase
    {
        public List<Binding> Bindings { get; } = new List<Binding>();

        public object DataContext { get; private set; } = default!;

        protected TViewModel ViewModel => (TViewModel) DataContext;

        void IBindable.SetDataContext(object dataContext)
        {
            DataContext = dataContext;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RestoreViewModelIfNeeded(savedInstanceState);

            OnViewModelRestored(ViewModel);

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

        protected virtual void RestoreViewModelIfNeeded(Bundle savedInstanceState)
        {
            if (ViewModel == null && savedInstanceState != null)
            {
                DataContext = Internal.ViewModelStore.Of(this).Get<TViewModel>(GetType().Name);
            }
        }

        protected virtual void DoAttachBindings()
        {
        }

        protected virtual void DoDetachBindings()
        {
            this.DetachBindings();
        }

        protected virtual void OnViewModelRestored(TViewModel viewModel)
        {
        }
    }
}

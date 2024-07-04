// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using AndroidX.Fragment.App;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    public class FragmentBase<TViewModel> : Fragment, IBindable
        where TViewModel : ViewModelBase
    {
        private readonly List<IDisposable> _commandsSubscriptions = new();

        public List<Binding> Bindings { get; } = new List<Binding>();

        public object DataContext { get; private set; } = default!;

        protected TViewModel ViewModel => (TViewModel) DataContext;

        void IBindable.SetDataContext(object dataContext)
        {
            DataContext = dataContext;
        }

        public override void OnCreate(Bundle? savedInstanceState)
        {
            RestoreViewModelIfNeeded(savedInstanceState);

            OnViewModelRestored();

            if (!ViewModel.IsInitialized)
            {
                ViewModel.OnInitialize();
            }

            // Calling base.OnCreate initiates restoring nested Fragments
            // so we should restore and initialize ViewModel before calling it
            base.OnCreate(savedInstanceState);
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

        protected virtual void RestoreViewModelIfNeeded(Bundle? savedInstanceState)
        {
            if (ViewModel == null && savedInstanceState != null)
            {
                var viewModelStore = Internal.ViewModelStore.Of(ParentFragmentManager);
                var key = Internal.ViewModelStore.GenerateKeyForType(typeof(TViewModel));
                DataContext = viewModelStore.Get<TViewModel>(key);
            }
        }

        protected virtual void DoAttachBindings()
        {
            var commandsSubscriptions = SetCommandsWithDisposing();
            _commandsSubscriptions.AddRange(commandsSubscriptions);
        }

        protected virtual void DoDetachBindings()
        {
            this.DetachBindings();

            _commandsSubscriptions.Apply(x => x.Dispose());
            _commandsSubscriptions.Clear();
        }

        protected virtual IEnumerable<IDisposable> SetCommandsWithDisposing()
        {
            return [];
        }

        protected virtual void OnViewModelRestored()
        {
        }
    }
}

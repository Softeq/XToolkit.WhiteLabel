// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Android.OS;
using Android.Support.V4.App;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Droid.Internal;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    public class FragmentBase<TViewModel> : Fragment, IBindable
        where TViewModel : ViewModelBase
    {
        public List<Binding> Bindings { get; } = new List<Binding>();

        public object DataContext { get; private set; }

        protected TViewModel ViewModel => (TViewModel) DataContext;

        void IBindable.SetDataContext(object dataContext)
        {
            DataContext = dataContext;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            InitializeViewModel(savedInstanceState);

            Log();
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

            Log();
        }

        protected virtual void InitializeViewModel(Bundle savedInstanceState)
        {
            // when fragment was recreated by the system
            if (ViewModel == null && savedInstanceState != null)
            {
                DataContext = ViewModelCache.Get(this);
            }

            if (!ViewModel.IsInitialized)
            {
                ViewModel.OnInitialize();
            }
        }

        protected virtual void DoAttachBindings()
        {
        }

        protected virtual void DoDetachBindings()
        {
            this.DetachBindings();
        }

        // TODO YP: REMOVE
        private void Log(string message = null, [CallerMemberName] string callerName = null)
        {
            Android.Util.Log.Debug("XXXX", $"{typeof(TViewModel).Name}: {callerName}: {message}");
        }
    }
}

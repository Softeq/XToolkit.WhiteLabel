// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Android.OS;
using Android.Support.V4.App;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    internal static class ViewModelCache
    {
        private static readonly Dictionary<string, IViewModelBase> _map;

        static ViewModelCache()
        {
            _map = new Dictionary<string, IViewModelBase>();
        }

        internal static void Save(string fragmentName, IViewModelBase viewModel, bool force = false)
        {
            if (!_map.ContainsKey(fragmentName))
            {
                _map.Add(fragmentName, viewModel);
            }

            if (force)
            {
                _map[fragmentName] = viewModel;
            }
        }

        internal static IViewModelBase Find(string fragmentName)
        {
            _map.TryGetValue(fragmentName, out var viewModel);
            return viewModel;
        }

        internal static void Remove(string fragmentName)
        {
            _map.Remove(fragmentName);
        }
    }



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

            Log();

            if (ViewModel == null && savedInstanceState != null) // was recreated by the system
            {
                DataContext = ViewModelCache.Find(GetType().Name);
            }

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

            Log();

            Dispose();
        }

        protected virtual void DoAttachBindings()
        {
        }

        protected virtual void DoDetachBindings()
        {
            this.DetachBindings();
        }

        private void Log(string message = null, [CallerMemberName] string callerName = null)
        {
            Android.Util.Log.Debug("XXXX", $"{typeof(TViewModel).Name}: {callerName}: {message}");
        }
    }
}

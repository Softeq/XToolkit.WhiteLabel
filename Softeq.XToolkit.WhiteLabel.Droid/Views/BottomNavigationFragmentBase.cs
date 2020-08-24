// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.OS;
using Android.Views;
using Google.Android.Material.BottomNavigation;
using Softeq.XToolkit.WhiteLabel.Droid.ViewComponents;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Droid.Views
{
    public abstract class BottomNavigationFragmentBase<TViewModel, TKey> : ToolbarFragmentBase<TViewModel, TKey>
        where TViewModel : ToolbarViewModelBase<TKey>
    {
        private BottomNavigationComponentBase<TViewModel, TKey>? _bottomNavigationComponent;

        protected BottomNavigationComponentBase<TViewModel, TKey> BottomNavigationComponent
        {
            get => _bottomNavigationComponent ?? throw new ArgumentNullException(nameof(_bottomNavigationComponent));
            private set => _bottomNavigationComponent = value;
        }

        protected override ToolbarComponent<TViewModel, TKey> ToolbarComponent => BottomNavigationComponent.ToolbarComponent;

        protected override void OnViewModelRestored(TViewModel viewModel)
        {
            BottomNavigationComponent = CreateComponent();

            base.OnViewModelRestored(viewModel);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(BottomNavigationComponent.Layout, null);

            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            BottomNavigationComponent.Initialize(view.FindViewById<BottomNavigationView>, Context);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            _bottomNavigationComponent?.Detach();
            _bottomNavigationComponent = null;
        }

        protected abstract BottomNavigationComponentBase<TViewModel, TKey> CreateComponent();
    }
}

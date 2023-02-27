// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.OS;
using Google.Android.Material.BottomNavigation;
using Softeq.XToolkit.WhiteLabel.Droid.ViewComponents;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Droid.Views
{
    public abstract class BottomNavigationActivityBase<TViewModel, TKey> : ToolbarActivityBase<TViewModel, TKey>
        where TViewModel : ToolbarViewModelBase<TKey>
    {
        private BottomNavigationComponentBase<TViewModel, TKey>? _bottomNavigationComponent;

        protected BottomNavigationComponentBase<TViewModel, TKey> BottomNavigationComponent
        {
            get => _bottomNavigationComponent ?? throw new ArgumentNullException(nameof(_bottomNavigationComponent));
            private set => _bottomNavigationComponent = value;
        }

        protected override ToolbarComponent<TViewModel, TKey> ToolbarComponent => BottomNavigationComponent.ToolbarComponent;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            BottomNavigationComponent = CreateComponent();

            base.OnCreate(savedInstanceState);

            SetContentView(BottomNavigationComponent.Layout);

            BottomNavigationComponent.Initialize(FindViewById<BottomNavigationView>, this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _bottomNavigationComponent?.Detach();
            _bottomNavigationComponent = null;
        }

        protected abstract BottomNavigationComponentBase<TViewModel, TKey> CreateComponent();
    }
}

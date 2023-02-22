// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Playground.ViewModels.BottomTabs;
using Softeq.XToolkit.WhiteLabel.Droid.ViewComponents;
using Softeq.XToolkit.WhiteLabel.Droid.Views;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;

namespace Playground.Droid.Views.BottomTabs
{
    [Activity(Theme = "@style/AppTheme")]
    public class BottomTabsPageActivity : BottomNavigationActivityBase<BottomTabsPageViewModel, string>
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            BottomNavigationComponent.BottomNavigationView?.SetBackgroundColor(Color.White);
        }

        protected override BottomNavigationComponentBase<BottomTabsPageViewModel, string> CreateComponent()
        {
            return new PlaygroundBottomNavigationComponent(this, ViewModel, SupportFragmentManager);
        }

        private class PlaygroundBottomNavigationComponent : BottomNavigationComponentBase<BottomTabsPageViewModel, string>
        {
            private Context? _context;

            public PlaygroundBottomNavigationComponent(
                Context context,
                BottomTabsPageViewModel viewModel,
                FragmentManager fragmentManager)
                : base(viewModel, fragmentManager)
            {
                _context = context;
            }

            public override void Detach()
            {
                base.Detach();
                _context = null;
            }

            protected override int GetImageResourceId(string key)
            {
                if (_context == null)
                {
                    throw new ArgumentNullException(nameof(_context));
                }

                if (_context.Resources == null)
                {
                    throw new ArgumentNullException(nameof(Resources));
                }

                var iconIdentifier = string.Concat("ic_", key.ToLower());
                return _context.Resources.GetIdentifier(iconIdentifier, "drawable", _context.PackageName);
            }
        }
    }
}

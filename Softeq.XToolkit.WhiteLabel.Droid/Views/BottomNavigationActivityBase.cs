// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;
using Android.Support.Design.Widget;
using Softeq.XToolkit.WhiteLabel.Droid.Extensions;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Droid.Views
{
    public abstract class BottomNavigationActivityBase<TViewModel> : ToolbarActivityBase<TViewModel>
        where TViewModel : ToolbarViewModelBase
    {
        private BottomNavigationView _bottomNavigationView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_bottom_navigation);

            _bottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.activity_bottom_navigation_page_navigation_view);
            _bottomNavigationView.InflateMenu(MenuId);
            _bottomNavigationView.NavigationItemSelected += BottomNavigationViewNavigationItemSelected;
        }

        protected abstract int MenuId { get; }

        private void BottomNavigationViewNavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            var index = _bottomNavigationView.GetIndex(e.Item);
            TabSelected(index);
        }
    }
}

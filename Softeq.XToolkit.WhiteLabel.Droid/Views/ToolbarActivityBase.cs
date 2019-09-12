// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using Android.OS;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Droid.Views
{
    public abstract class ToolbarActivityBase<TViewModel> : ActivityBase<TViewModel>
        where TViewModel : ToolbarViewModelBase
    {
        protected abstract int NavigationContainer { get; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            var wasInit = ViewModel.IsInitialized;

            base.OnCreate(savedInstanceState);

            if (wasInit) // HACK YP: need another way
            {
                return;
            }

            foreach (var tabViewModel in ViewModel.TabViewModels)
            {
                tabViewModel.InitializeNavigation(NavigationContainer);
            }

            var selectedTabViewModel = ViewModel.TabViewModels.ElementAt(ViewModel.SelectedIndex);

            selectedTabViewModel.NavigateToFirstPage();
        }

        protected void TabSelected(int newSelectedIndex)
        {
            var oldSelectedIndex = ViewModel.SelectedIndex;

            ViewModel.SelectionChangedCommand?.Execute(newSelectedIndex);

            var selectedTabViewModel = ViewModel.TabViewModels.ElementAt(ViewModel.SelectedIndex);

            if (newSelectedIndex == oldSelectedIndex) // fast-backward nav
            {
                selectedTabViewModel.NavigateToFirstPage();
            }
            else
            {
                selectedTabViewModel.RestoreState();
            }
        }

        public override void OnBackPressed()
        {
            if (ViewModel.CanGoBack)
            {
                ViewModel.GoBackCommand.Execute(null);
            }
            else
            {
                base.OnBackPressed();
            }
        }
    }
}

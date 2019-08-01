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
            base.OnCreate(savedInstanceState);

            foreach (var tabViewModel in ViewModel.TabViewModels)
            {
                tabViewModel.InitializeNavigation(NavigationContainer);
            }

            ViewModel.TabViewModels.ElementAt(ViewModel.SelectedIndex).NavigateToFirstPage();
        }

        protected void TabSelected(int index)
        {
            var oldSelectedIndex = ViewModel.SelectedIndex;

            ViewModel.SelectionChangedCommand?.Execute(index);

            if (oldSelectedIndex == index)
            {
                ViewModel.TabViewModels.ElementAt(ViewModel.SelectedIndex).NavigateToFirstPage();
            }
            else
            {
                ViewModel.TabViewModels.ElementAt(ViewModel.SelectedIndex).RestoreState();
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

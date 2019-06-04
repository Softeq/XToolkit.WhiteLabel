using System;
using System.Linq;
using Android.OS;
using Android.Support.V4.App;
using Halo.Core.ViewModels.Tab;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Droid.Views
{
    public abstract class ToolbarActivityBase<TViewModel> : ActivityBase<TViewModel>
        where TViewModel : ToolbarViewModelBase
    {
        protected abstract int NavigationContainer { get; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            if (ViewModel.IsInitialized)
            {
                return;
            }
            foreach (var tabViewModel in ViewModel.Dictionary)
            {
                tabViewModel.InitializeNavigation(NavigationContainer);
            }
            base.OnCreate(savedInstanceState);
           
            ViewModel.Dictionary.ElementAt(ViewModel.SelectedViewModel).NavigateToFirstPage();
        }

        protected void TabSelected(int num)
        {
            ViewModel.SelectionChangedCommand?.Execute(num);

            ViewModel.Dictionary.ElementAt(ViewModel.SelectedViewModel).NavigateToFirstPage();
        }
    }
}

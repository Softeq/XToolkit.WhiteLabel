// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.MasterDetailNavigation.DrillNavigation
{
    public class DrillRootPageViewModel : RootFrameNavigationViewModelBase
    {
        public DrillRootPageViewModel(IFrameNavigationService frameNavigationService)
            : base(frameNavigationService)
        {
        }

        public override void NavigateToFirstPage()
        {
            FrameNavigationService
                .For<DrillLevel1PageViewModel>()
                .Navigate(true);
        }
    }
}

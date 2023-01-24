// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Flows
{
    public class FlowViewModel : DissmissableDialogComponentViewModel
    {
        public FlowViewModel(
            IFrameNavigationService frameNavigationService)
        {
            FrameNavigationService = frameNavigationService;
        }

        public IFrameNavigationService FrameNavigationService { get; }

        public ICommand NavigateToFirstScreenCommand { get; set; }

        public void InitializeNavigation(object navigator)
        {
            FrameNavigationService.Initialize(navigator);
            NavigateToFirstScreenCommand?.Execute(null);
        }
    }
}

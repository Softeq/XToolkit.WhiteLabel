// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.Flows;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.TestApproach2.Flows
{
    public class MainFlow : FlowBase
    {
        // Maybe make sense to put all navigation directions here in the flow and flow will be intirely responsible
        // for navigation directions
        // But it not solve a problem when we need to navigation to different view models in that case NextCommand
        // will not work because there will be multiple Nexts
        // in theory we can add parameter and according to that parameter next will behave differently,
        // but here is another issue how to properly manage this magic states

        private readonly NavigationDirection _navigationDirection;

        public MainFlow(NavigationDirection navigationDirection)
        {
            _navigationDirection = navigationDirection;
        }

        protected override void NavigateToFirstStep()
        {
            FrameNavigationService.For<ProvNameViewModel>()
                .WithParam(vm => vm.NavigationDirection, _navigationDirection)
                .Navigate();
        }

        public void NavigateToDetails(string obj, NavigationDirection navigationDirection)
        {
            FrameNavigationService.For<NameDetailsViewModel>()
                .WithParam(vm => vm.Name, obj)
                .WithParam(vm => vm.NavigationDirection, navigationDirection)
                .Navigate();
        }
    }
}

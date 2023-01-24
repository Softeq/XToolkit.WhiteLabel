using System;
using Playground.ViewModels.Flows;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.TestApproach2.Flows
{
    public class SecondEditFlow : FlowBase
    {
        private readonly NavigationDirection _initialNavigationDirection;

        public SecondEditFlow(NavigationDirection initialNavigationDirection)
        {
            _initialNavigationDirection = initialNavigationDirection;
        }

        protected override void NavigateToFirstStep()
        {
            FrameNavigationService.For<ProvNameViewModel>()
                           .WithParam(vm => vm.NavigationDirection, _initialNavigationDirection)
                           .Navigate();
        }
    }
}

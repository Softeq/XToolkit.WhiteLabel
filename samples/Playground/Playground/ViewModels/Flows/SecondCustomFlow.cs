// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Flows
{
    public class SecondCustomFlow : FlowBase
    {
        public SecondCustomFlow()
        {
        }

        protected override void NavigateToFirstStep()
        {
            FrameNavigationService.For<SecondCustomViewModel>()
                .WithParam(x => x.CurrentFlow, this)
                .Navigate();
        }

        public void NavigateToSecondStep()
        {
            FrameNavigationService.For<ThirdCustomViewModel>()
                .WithParam(x => x.CurrentFlow, this)
                .Navigate();
        }
    }
}

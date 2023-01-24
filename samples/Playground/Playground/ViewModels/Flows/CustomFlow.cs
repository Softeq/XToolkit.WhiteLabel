// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Flows
{
    public class CustomFlow : FlowBase
    {
        protected override void NavigateToFirstStep()
        {
            FrameNavigationService.For<FirstCustomViewModel>()
                .WithParam(x => x.CurrentFlow, this)
                .Navigate();
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.AddEditProfile;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Flows
{
    public class EditFlow : FlowBase
    {
        protected override void NavigateToFirstStep()
        {
            FrameNavigationService.For<ProvideProfileInfoViewModelEditFlowDecorator>()
                .WithParam(x => x.CurrentFlow, this)
                .Navigate();
        }
    }
}

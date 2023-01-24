// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.AddEditProfile;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Flows
{
    public class EditProfileFlow : FlowBase
    {
        public EditProfileFlow()
        {
        }

        protected override void NavigateToFirstStep()
        {
            FrameNavigationService.For<ProvideProfileInfoViewModelEditProfileFlowDecorator>()
                .WithParam(x => x.CurrentFlow, this)
                .Navigate();
        }

        public void NavigateToProfileDetailsScreen(string name)
        {
            FrameNavigationService.For<ProfileDetailsViewModel>()
                .WithParam(x => x.CurrentFlow, this)
                .WithParam(x => x.Name, name)
                .Navigate();
        }
    }
}

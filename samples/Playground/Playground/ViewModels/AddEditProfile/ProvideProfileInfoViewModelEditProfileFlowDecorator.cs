// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.Flows;

namespace Playground.ViewModels.AddEditProfile
{
    public class ProvideProfileInfoViewModelEditProfileFlowDecorator : ProvideProfileInfoViewModel
    {
        public EditProfileFlow CurrentFlow { get; set; }

        protected override void Add()
        {
            CurrentFlow.NavigateToProfileDetailsScreen(Name);
        }
    }
}

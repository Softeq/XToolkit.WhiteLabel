using System;
using Playground.ViewModels.Flows;
using Softeq.XToolkit.Common.Extensions;

namespace Playground.ViewModels.AddEditProfile
{
    public class ProvideProfileInfoViewModelEditFlowDecorator : ProvideProfileInfoViewModel
    {
        public EditFlow CurrentFlow { get; set; }

        protected override void Add()
        {
            CurrentFlow.Finish().FireAndForget();
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Playground.ViewModels.Flows;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.TestApproach.Flows
{
    public class EditTestApproachFlow : FlowBase
    {
        private readonly string _name;
        private readonly TaskCompletionSource<string> _result;

        public EditTestApproachFlow(string name, TaskCompletionSource<string> tcs)
        {
            _name = name;
            _result = tcs;
        }

        protected override async void NavigateToFirstStep()
        {
            FrameNavigationService.For<EditNameViewModel>()
                .WithParam(x => x.Name, _name)
                .WithParam(x => x.CompletionSource, _result)
                .Navigate();
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Playground.ViewModels.Flows;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.TestApproach.Flows
{
    public class MainAddDetailFlow : FlowBase
    {
        public class FlowState
        {

        }

        private readonly TaskCompletionSource<string> _firstStepResult;

        public MainAddDetailFlow(TaskCompletionSource<string> firstStepResult)
        {
            _firstStepResult = firstStepResult;
        }

        protected override async void NavigateToFirstStep()
        {
            FrameNavigationService.For<EditNameViewModel>()
                .WithParam(x => x.CompletionSource, _firstStepResult)
                .Navigate();
        }

        public void NavigateToDetail(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            FrameNavigationService.For<DetailViewModel>()
                .WithParam(x => x.Name, name)
                .Navigate();
        }
    }
}

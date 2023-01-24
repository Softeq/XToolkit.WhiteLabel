using System;
using System.Threading.Tasks;
using Playground.ViewModels.Flows;
using Playground.ViewModels.TestApproach;
using Playground.ViewModels.TestApproach2.Flows;
using Softeq.XToolkit.Common.Commands;

namespace Playground.ViewModels.TestApproach2
{
    public class NavigationService : ICommonNavigationService
    {
        private readonly IFlowService _flowService;
        private MainFlow mainFlow;
        private SecondEditFlow editFlow;

        public NavigationService(
            IFlowService flowService)
        {
            _flowService = flowService;
        }

        public async Task<string> NavigateToEditViewModel(string name)
        {
            var directionToDetail = new NavigationDirection
            {
                Next = new RelayCommand(() => NavigateToProvideNameFlow()),
            };

            mainFlow = new MainFlow(new NavigationDirection
            {
                Next = new RelayCommand<object>((obj) => mainFlow.NavigateToDetails((string)obj, directionToDetail)),
                Back = new RelayCommand(() => mainFlow.Finish())
            });

            await _flowService.ProcessAsync(mainFlow);
            return string.Empty;
        }

        public async void NavigateToProvideNameFlow()
        {
            var directionToDetail = new NavigationDirection
            {
                Next = new AsyncCommand(() => editFlow.Finish()),
                Back = new AsyncCommand(() => editFlow.Finish()),
            };

            editFlow = new SecondEditFlow(directionToDetail);
            await _flowService.ProcessAsync(editFlow);
        }
    }
}

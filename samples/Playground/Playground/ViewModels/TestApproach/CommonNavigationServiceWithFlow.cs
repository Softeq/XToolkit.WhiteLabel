// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Playground.ViewModels.Flows;
using Playground.ViewModels.TestApproach.Flows;
using Softeq.XToolkit.Common.Extensions;

namespace Playground.ViewModels.TestApproach
{
    public class CommonNavigationServiceWithFlow : ICommonNavigationService
    {
        private readonly IFlowService _flowService;

        public CommonNavigationServiceWithFlow(
            IFlowService flowService)
        {
            _flowService = flowService;
        }

        public async Task<string> NavigateToEditViewModel(string name)
        {
            TaskCompletionSource<string> editCompletionSource = new TaskCompletionSource<string>();
            var editFlow = new EditTestApproachFlow(name, editCompletionSource);
            await Task.WhenAny(_flowService.ProcessAsync(editFlow), editCompletionSource.Task);

            if (editCompletionSource.Task.IsCompleted)
            {
                string fixResult = await editCompletionSource.Task;
                await editFlow.Finish();

                return fixResult;
            }

            return name;
        }

        public async void NavigateToProvideNameFlow()
        {
            var tcs = new TaskCompletionSource<string>();
            var mainAddDetailFlow = new MainAddDetailFlow(tcs);
            _flowService.ProcessAsync(mainAddDetailFlow).FireAndForget();
            string result = await tcs.Task;

            if (!string.IsNullOrWhiteSpace(result))
            {
                mainAddDetailFlow.NavigateToDetail(result);
            }
        }
    }
}

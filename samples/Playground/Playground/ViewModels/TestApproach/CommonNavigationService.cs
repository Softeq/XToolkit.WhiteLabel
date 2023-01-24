// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.TestApproach
{
    public class CommonNavigationService : ICommonNavigationService
    {
        private readonly IPageNavigationService _pageNavigationService;
        private readonly IFrameNavigationService _frameNavigationService;

        public CommonNavigationService(
            IPageNavigationService pageNavigationService,
            IFrameNavigationService frameNavigationService)
        {
            _pageNavigationService = pageNavigationService;
            _frameNavigationService = frameNavigationService;
        }

        public async Task<string> NavigateToEditViewModel(string name)
        {
            var taskCompletionSource = new TaskCompletionSource<string>();
            _frameNavigationService.For<EditNameViewModel>()
                .WithParam(x => x.Name, name)
                .WithParam(x => x.CompletionSource, taskCompletionSource)
                .Navigate();

            return await taskCompletionSource.Task;
        }

        public async void NavigateToProvideNameFlow()
        {
            var taskCompletionSource = new TaskCompletionSource<string>();
            _frameNavigationService
                .For<EditNameViewModel>()
                .WithParam(cs => cs.CompletionSource, taskCompletionSource)
                .Navigate();

            string name = await taskCompletionSource.Task;
            if (name != null)
            {
                _frameNavigationService
                    .For<DetailViewModel>()
                    .WithParam(x => x.Name, name)
                    .Navigate();
            }
        }
    }
}

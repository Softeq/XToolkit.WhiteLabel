// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.CoordinatorPattern.ViewModels;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.CoordinatorPattern
{
    public class EditDetailCoordinator : ICoordinator
    {
        private readonly string _name;
        private readonly IFrameNavigationService _frameNavigationService;

        public EditDetailCoordinator(
            string name,
            IFrameNavigationService frameNavigationService)
        {
            _name = name;
            _frameNavigationService = frameNavigationService;
        }

        public void Finish()
        {
        }

        public void PresentChild()
        {
        }

        public void Start()
        {
            Action<string> finishWithResult = new Action<string>(result =>
            {
                _frameNavigationService.GoBack();
            });

            _frameNavigationService
                .For<CoordinatorEditViewModel>()
                .WithParam(x => x.Name, _name)
                .WithParam(x => x.OnNext, finishWithResult)
                .Navigate();
        }
    }
}

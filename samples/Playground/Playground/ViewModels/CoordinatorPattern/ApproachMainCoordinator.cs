// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.CoordinatorPattern.ViewModels;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.CoordinatorPattern
{
    public class ApproachMainCoordinator : ICoordinator
    {
        private EditDetailCoordinator _editDetailCoordinator;

        private readonly IFrameNavigationService _frameNavigationService;

        public ApproachMainCoordinator(IFrameNavigationService frameNavigationService)
        {
            _frameNavigationService = frameNavigationService;
        }

        public void Finish()
        {
        }

        public void Start()
        {
            _frameNavigationService
                .For<CoordinatorEditViewModel>()
                .WithParam(x => x.OnNext, PresentChild)
                .Navigate();
        }

        private void PresentChild(string name)
        {
            Action<string> edit = new Action<string>(result =>
            {
                _editDetailCoordinator = new EditDetailCoordinator(result, _frameNavigationService);
                _editDetailCoordinator.Start();
            });

            _frameNavigationService
                  .For<CoordinatorDetailViewModel>()
                  .WithParam(x => x.Name, name)
                  .WithParam(x => x.Edit, edit)
                  .Navigate();
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows.Input;
using Playground.ViewModels.BLoC.Flows;
using Playground.ViewModels.CoordinatorPattern;
using Playground.ViewModels.Flows;
using Playground.ViewModels.RIB.Login;
using Playground.ViewModels.RIB.Platform.Navigation;
using Playground.ViewModels.RIB.Root;
using Playground.ViewModels.TestApproach;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels
{
    [SuppressMessage("ReSharper", "EmptyDestructor", Justification = "Just for play.")]
    [SuppressMessage("ReSharper", "RedundantOverriddenMember", Justification = "Just for play.")]
    [SuppressMessage("ReSharper", "EmptyConstructor", Justification = "Just for play.")]
    public class EmptyPageViewModel : ViewModelBase, IRootListener
    {
        private readonly IRibNavigationService _ribNavigation;
        private readonly IPageNavigationService _pageNavigationService;
        private readonly IFrameNavigationService _frameNavigationService;
        private readonly IFlowService _flowService;
        private readonly ICommonNavigationService _commonNavigationService;

        public EmptyPageViewModel(
            IRibNavigationService ribNavigation,
            IPageNavigationService pageNavigationService,
            IFrameNavigationService frameNavigationService,
            IFlowService flowService,
            ICommonNavigationService commonNavigationService)
        {
            _ribNavigation = ribNavigation;
            _pageNavigationService = pageNavigationService;
            _frameNavigationService = frameNavigationService;
            _flowService = flowService;
            _commonNavigationService = commonNavigationService;
            OpenCustomFlowCommand = new AsyncCommand(OpenCustomFlow);
            OpenEditProfileNameFlowCommand = new AsyncCommand(OpenEditProfileFlow);
            OpenTestApproachCommand = new RelayCommand(OpenTestApproach);
            OpenTestApproachWithFlowCommand = new RelayCommand(OpenTestApproachWithFlow);
            OpenRibFlowCommand = new RelayCommand(OpenRibFlow);
        }

        ~EmptyPageViewModel()
        {
        }

        public ICommand OpenCustomFlowCommand { get; }
        public ICommand OpenEditProfileNameFlowCommand { get; }
        public ICommand OpenTestApproachCommand { get; }
        public ICommand OpenTestApproachWithFlowCommand { get; }
        public ICommand OpenRibFlowCommand { get; set; }

        public override void OnAppearing()
        {
            base.OnAppearing();

            // Put your code HERE.
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            // Put your code HERE.
        }

        private ICommonNavigationService _commonNavigationWithFlow;
        private IRootRouting _rootRouter;

        private void OpenTestApproachWithFlow()
        {
            _commonNavigationWithFlow = new CommonNavigationServiceWithFlow(_flowService);
            _commonNavigationWithFlow.NavigateToProvideNameFlow();
        }

        private void OpenTestApproach()
        {
            var provideNameFlow = new ProvideNameFlow(_flowService);
            _flowService.ProcessAsync(provideNameFlow);
        }

        private async Task OpenCustomFlow()
        {
            var customFlow = new CustomFlow();
            await _flowService.ProcessAsync(customFlow);
        }

        private async Task OpenEditProfileFlow()
        {
            var editProfileFlow = new EditProfileFlow();
            await _flowService.ProcessAsync(editProfileFlow);
        }

        private void OpenRibFlow()
        {
            var builder = new RootBuilder();
            _rootRouter = builder.Build(this);
            _rootRouter.RouteToFirstView();
        }

        public void DismissRootFlow()
        {
        }

        public void Logout()
        {
        }
    }
}

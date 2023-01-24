using System;
using System.Collections.Generic;
using Playground.ViewModels.RIB.CreateAccount;
using Playground.ViewModels.RIB.Detail;
using Playground.ViewModels.RIB.Login;
using Playground.ViewModels.RIB.Platform;
using Playground.ViewModels.RIB.Platform.Navigation;
using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.RIB.Main
{
    public interface IMainInteractable : IInteractable
    {
        IMainRouting Router { get; set; }
        IMainListener Listener { get; set; }
    }

    public class MainRouter : ViewvableRouter<IMainInteractable>, IMainRouting
    {
        private readonly IRibNavigationService _ribNavigation;
        private IDetailBuildable _detailBuilder;
        private IDetailRouting _detailRouter;

        public MainRouter(
            IRibNavigationService ribNavigation)
        {
            _ribNavigation = ribNavigation;
        }

        public void DismissMainFlow()
        {
            throw new NotImplementedException();
        }

        public void Init(
            IMainInteractable interactable,
            ViewModelBase viewModel,
            IDetailBuildable detailBuilder)
        {
            this.Init(interactable, viewModel);
            interactable.Router = this;

            _detailBuilder = detailBuilder;
        }

        public void RouteToDetails()
        {
            _detailRouter = _detailBuilder.Build((IDetailListener) Interactable);
            AttachChild(_detailRouter);
            _ribNavigation.NavigateToViewModel(_detailRouter.ViewModel);
        }
    }
}

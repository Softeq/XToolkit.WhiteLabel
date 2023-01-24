// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Playground.ViewModels.RIB.CreateAccount;
using Playground.ViewModels.RIB.Main;
using Playground.ViewModels.RIB.Platform;
using Playground.ViewModels.RIB.Platform.Navigation;
using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.RIB.Login
{
    public interface ILoginInteractable : IInteractable
    {
        ILoginRouting Router { get; set; }
        ILoginListener Listener { get; set; }
    }

    public class LoginRouter : ViewvableRouter<ILoginInteractable>,
        ILoginRouting
    {
        private readonly IRibNavigationService _ribNavigationService;

        private ICreateAccountBuildable _createAccountBuilder;
        private IMainBuildable _mainBuildable;
        private ICreateAccountRouting _createAccountRouter;
        private IMainRouting _mainRouting;

        public LoginRouter(
            IRibNavigationService ribNavigationService)
        {
            _ribNavigationService = ribNavigationService;
        }

        public void Init(
            ILoginInteractable interactor,
            ViewModelBase viewModel,
            ICreateAccountBuildable createAccountBuilder,
            IMainBuildable mainBuildable)
        {
            this.Init(interactor, viewModel);

            _createAccountBuilder = createAccountBuilder;
            _mainBuildable = mainBuildable;

            interactor.Router = this;
        }

        public void RouteToMain()
        {
            _mainRouting = _mainBuildable.Build((IMainListener) Interactable);
            AttachChild(_mainRouting);
            _ribNavigationService.NavigateToViewModel(_mainRouting.ViewModel);
        }

        public void RouteToCreateAccount()
        {
            _createAccountRouter = _createAccountBuilder.Build((ICreateAccountListener) Interactable);
            AttachChild(_createAccountRouter);
            _ribNavigationService.NavigateToViewModel(_createAccountRouter.ViewModel);
        }

        public void DetachCreateAccount()
        {
            DetachChild(_createAccountRouter);
        }
    }
}

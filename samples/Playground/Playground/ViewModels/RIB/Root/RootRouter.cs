// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.ViewModels.RIB.Login;
using Playground.ViewModels.RIB.Main;
using Playground.ViewModels.RIB.Platform;
using Playground.ViewModels.RIB.Platform.Navigation;

namespace Playground.ViewModels.RIB.Root
{
    public interface IRootInteractable : IInteractable
    {
        IRootRouting Router { get; set; }
        IRootListener Listener { get; set; }
    }

    public class RootRouter : Router<IRootInteractable>, IRootRouting
    {
        private readonly ILoginBuildable _loginBuilder;
        private readonly IMainBuildable _mainBuilder;
        private readonly IRibNavigationService _ribNavigationService;

        private IMainRouting _mainRouter;
        private ILoginRouting _loginRouter;

        public RootRouter(
            ILoginBuildable loginBuilder,
            IMainBuildable mainBuilder,
            IRibNavigationService ribNavigationService)
        {
            _loginBuilder = loginBuilder;
            _mainBuilder = mainBuilder;
            _ribNavigationService = ribNavigationService;
        }

        public void RouteToFirstView()
        {
            // Navigate to Splash

            if (true)
            {
                NavigateToLogin();
            }
            else
            {
                NavigateToMain();
            }
        }

        private void NavigateToSpash()
        {
            // TODO: Implement spash
        }

        public void NavigateToLogin()
        {
            _loginRouter = _loginBuilder.Build((ILoginListener) Interactable);
            AttachChild(_loginRouter);
            _ribNavigationService.NavigateToViewModel(_loginRouter.ViewModel);
        }

        public void NavigateToMain()
        {
            _mainRouter = _mainBuilder.Build((IMainListener) Interactable);
            AttachChild(_mainRouter);
            _ribNavigationService.NavigateToViewModel(_mainRouter.ViewModel);
        }

        public void Logout()
        {
            NavigateToLogin();

            // Remove all child except login
        }
    }
}

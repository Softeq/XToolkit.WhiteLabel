// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.ViewModels.RIB.Main;
using Playground.ViewModels.RIB.Platform;
using Playground.ViewModels.RIB.Platform.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.RIB.CreateAccount
{
    public interface ICreateAccountInteractable : IInteractable
    {
        ICreateAccountRouting Router { get; set; }
        ICreateAccountListener Listener { get; set; }
    }

    public class CreateAccountRouter : ViewvableRouter<ICreateAccountInteractable>, ICreateAccountRouting
    {
        private readonly IRibNavigationService _ribNavigationService;
        private IMainBuildable _mainBuildable;
        private IMainRouting _mainRouting;

        public CreateAccountRouter(
            IRibNavigationService ribNavigationService)
        {
            _ribNavigationService = ribNavigationService;
        }

        public void Init(
            IMainBuildable mainBuildable,
            ICreateAccountInteractable interactor,
            ViewModelBase viewModel)
        {
            this.Init(interactor, viewModel);

            interactor.Router = this;

            _mainBuildable = mainBuildable;
        }

        public void RouteToMain()
        {
            _mainRouting = _mainBuildable.Build((IMainListener) Interactable);
            AttachChild(_mainRouting);
            _ribNavigationService.NavigateToViewModel(_mainRouting.ViewModel);
        }
    }
}

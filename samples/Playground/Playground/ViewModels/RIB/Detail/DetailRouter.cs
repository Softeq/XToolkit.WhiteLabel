// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.ViewModels.RIB.CreateAccount;
using Playground.ViewModels.RIB.Platform;
using Playground.ViewModels.RIB.Platform.Navigation;
using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.RIB.Detail
{
    public interface IDetailInteractable : IInteractable, IDetailListener
    {
        IDetailRouting Router { get; set; }
        IDetailListener Listener { get; set; }
    }

    public class DetailRouter : ViewvableRouter<IDetailInteractable>, IDetailRouting
    {
        private readonly IRibNavigationService _ribNavigationService;

        public DetailRouter(
            IRibNavigationService ribNavigationService)
        {
            _ribNavigationService = ribNavigationService;
        }

        public override void Init(IDetailInteractable interactable, ViewModelBase viewModel)
        {
            base.Init(interactable, viewModel);

            interactable.Router = this;
        }

        public void DetachDetailFlow()
        {
            _ribNavigationService.GoBack();

            DetachChild(this);
        }
    }
}

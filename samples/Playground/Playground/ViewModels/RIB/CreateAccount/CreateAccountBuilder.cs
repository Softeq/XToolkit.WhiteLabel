// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.ViewModels.RIB.Platform;
using Softeq.XToolkit.WhiteLabel;

namespace Playground.ViewModels.RIB.CreateAccount
{
    public interface ICreateAccountBuildable
    {
        ICreateAccountRouting Build(ICreateAccountListener listener);
    }

    public class CreateAccountBuilder : ICreateAccountBuildable
    {
        public ICreateAccountRouting Build(ICreateAccountListener listener)
        {
            var viewModel = Dependencies.Container.Resolve<CreateAccountViewModel>();

            var interactor = new CreateAccountInteractor();
            interactor.Init(viewModel);
            interactor.Listener = listener;

            var router = Dependencies.Container.Resolve<CreateAccountRouter>();
            router.Init(interactor, viewModel);

            return router;
        }
    }
}

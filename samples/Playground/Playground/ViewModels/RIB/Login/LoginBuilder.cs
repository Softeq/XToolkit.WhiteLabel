// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.ViewModels.RIB.CreateAccount;
using Playground.ViewModels.RIB.Main;
using Playground.ViewModels.RIB.Platform;
using Softeq.XToolkit.WhiteLabel;

namespace Playground.ViewModels.RIB.Login
{
    public interface ILoginBuildable
    {
        ILoginRouting Build(ILoginListener listener);
    }

    public class LoginBuilder : ILoginBuildable
    {
        public ILoginRouting Build(ILoginListener listener)
        {
            var viewModel = Dependencies.Container.Resolve<LoginViewModel>();
            var interactor = Dependencies.Container.Resolve<LoginInteractor>();
            interactor.Init(viewModel);
            interactor.Listener = listener;

            var createAccountBuilder = new CreateAccountBuilder();
            var mainBuilder = new MainBuilder();

            var router = Dependencies.Container.Resolve<LoginRouter>();

            router.Init(interactor, viewModel, createAccountBuilder, mainBuilder);

            return router;
        }
    }
}

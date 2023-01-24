// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.ViewModels.RIB.Detail;
using Playground.ViewModels.RIB.Platform;
using Softeq.XToolkit.WhiteLabel;

namespace Playground.ViewModels.RIB.Main
{
    public interface IMainBuildable
    {
        IMainRouting Build(IMainListener listener);
    }

    public class MainBuilder : IMainBuildable
    {
        public IMainRouting Build(IMainListener listener)
        {
            var viewModel = Dependencies.Container.Resolve<MainViewModel>();

            var interactor = new MainInteractor();
            interactor.Init(viewModel);
            interactor.Listener = listener;

            var detailBuilder = new DetailBuilder();

            var router = Dependencies.Container.Resolve<MainRouter>();
            router.Init(interactor, viewModel, detailBuilder);

            return router;
        }
    }
}

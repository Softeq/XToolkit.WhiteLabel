// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.ViewModels.RIB.Main;
using Playground.ViewModels.RIB.Platform;
using Softeq.XToolkit.WhiteLabel;

namespace Playground.ViewModels.RIB.Detail
{
    public interface IDetailBuildable
    {
        IDetailRouting Build(IDetailListener listener);
    }

    public class DetailBuilder : IDetailBuildable
    {
        public IDetailRouting Build(IDetailListener listener)
        {
            var viewModel = Dependencies.Container.Resolve<DetailViewModel>();
            var interactor = new DetailInteractor();
            interactor.Init(viewModel);
            interactor.Listener = listener;

            var detailRouter = Dependencies.Container.Resolve<DetailRouter>();
            detailRouter.Init(interactor, viewModel);

            return detailRouter;
        }
    }
}
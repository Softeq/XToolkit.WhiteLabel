// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.ViewModels.RIB.Detail;
using Playground.ViewModels.RIB.Main;
using Playground.ViewModels.RIB.Platform;
using Softeq.XToolkit.WhiteLabel;

namespace Playground.ViewModels.RIB.Root
{
    public interface IRootBuildable
    {
        IRootRouting Build(IRootListener listener);
    }

    public class RootBuilder : IRootBuildable
    {
        public IRootRouting Build(IRootListener listener)
        {
            var interactor = new RootInteractor();
            interactor.Listener = listener;

            var router = Dependencies.Container.Resolve<RootRouter>();

            return router;
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.ViewModels.RIB.Login;
using Playground.ViewModels.RIB.Main;
using Playground.ViewModels.RIB.Platform;

namespace Playground.ViewModels.RIB.Root
{
    public interface IRootRouting : IRouting
    {
        void RouteToFirstView();
    }

    public interface IRootListener
    {
        void DismissRootFlow();
        void Logout();
    }

    public class RootInteractor : Interactor, IRootInteractable
    {
        public IRootRouting Router { get; set; }
        public IRootListener Listener { get; set; }
    }
}

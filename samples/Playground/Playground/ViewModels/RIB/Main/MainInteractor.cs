// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.ViewModels.RIB.Platform;

namespace Playground.ViewModels.RIB.Main
{
    public interface IMainRouting : IViewvableRouter
    {
        void RouteToDetails();
    }

    public interface IMainListener
    {
        void DismissMainFlow();
        void Save(string value);
    }

    public interface IMainPresentable
    {
        IMainPresentableListener? Listener { get; set; }
    }

    public class MainInteractor : PresentableInteractor<IMainPresentable>,
        IMainInteractable,
        IMainPresentableListener
    {
        public IMainRouting Router { get; set; }
        public IMainListener Listener { get; set; }

        public override void Init(IMainPresentable presenter)
        {
            base.Init(presenter);

            presenter.Listener = this;
        }

        public void DidTapSave(string value)
        {
            Listener?.Save(value);
            Router?.RouteToDetails();
        }
    }
}

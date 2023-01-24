// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.ViewModels.RIB.Platform;

namespace Playground.ViewModels.RIB.Detail
{
    public interface IDetailRouting : IViewvableRouter
    {
        void DetachDetailFlow();
    }

    public interface IDetailListener
    {
        void CloseDetailFlow();
        void Save(string value);
    }

    public interface IDetailPresentable
    {
        IDetailPresentableListener? Listener { get; set; }
    }

    public class DetailInteractor : PresentableInteractor<IDetailPresentable>,
        IDetailInteractable,
        IDetailPresentableListener
    {
        public IDetailRouting Router { get; set; }
        public IDetailListener Listener { get; set; }

        public override void Init(IDetailPresentable presenter)
        {
            base.Init(presenter);

            presenter.Listener = this;
        }

        public void CloseDetailFlow()
        {
            Router?.DetachDetailFlow();
        }

        public void DidTapClose()
        {
            Listener?.CloseDetailFlow();
        }

        public void DidTapSave(string value)
        {
            Listener?.Save(value);
        }

        public void Save(string value)
        {
        }
    }
}

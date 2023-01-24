// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Playground.ViewModels.RIB.Platform
{
    public class PresentableInteractor<TPresentationType> : Interactor
    {
        public virtual void Init(TPresentationType presenter)
        {
            Presenter = presenter;
        }

        public TPresentationType Presenter { get; private set; }
    }
}

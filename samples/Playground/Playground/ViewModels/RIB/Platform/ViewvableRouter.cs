// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.RIB.Platform
{
    public interface IViewvableRouter : IRouting
    {
        ViewModelBase ViewModel { get; }
    }

    public class ViewvableRouter<TInteractorType> : Router<TInteractorType>, IViewvableRouter
        where TInteractorType : IInteractable
    {
        public virtual void Init(TInteractorType interactor, ViewModelBase viewModel)
        {
            this.Init(interactor);

            ViewModel = viewModel;
        }

        public ViewModelBase ViewModel { get; private set; }
    }
}

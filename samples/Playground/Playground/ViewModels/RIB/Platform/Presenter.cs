// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Playground.ViewModels.RIB.Platform
{
    public class Presenter<TViewModelType>
    {
        public Presenter(TViewModelType viewModel)
        {
            ViewModel = viewModel;
        }

        public TViewModelType ViewModel { get; }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Interfaces
{
    public interface IViewModelFactoryService
    {
        TViewModel ResolveViewModel<TViewModel, TParam>(TParam param)
            where TViewModel : ObservableObject, IViewModelParameter<TParam>;

        TViewModel ResolveViewModel<TViewModel>()
            where TViewModel : ObservableObject;
    }
}
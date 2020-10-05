// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public interface IViewModelBase
    {
        bool IsInitialized { get; }

        void OnInitialize();
        void OnAppearing();
        void OnDisappearing();
    }
}

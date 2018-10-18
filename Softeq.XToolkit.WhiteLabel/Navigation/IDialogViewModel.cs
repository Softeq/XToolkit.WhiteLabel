// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IDialogViewModel
    {
        DialogViewModelComponent DialogComponent { get; }
        
        void OnInitialize();
        
        void OnAppearing();
        
        void OnDisappearing();
    }
}
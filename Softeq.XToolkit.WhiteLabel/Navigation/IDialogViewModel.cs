// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IDialogViewModel : IViewModelBase
    {
        DialogViewModelComponent DialogComponent { get; }
    }
}
// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public class DialogViewModelBase : ViewModelBase, IDialogViewModel
    {
        protected DialogViewModelBase()
        {
            DialogComponent = new DialogViewModelComponent();
        }

        public DialogViewModelComponent DialogComponent { get; }
    }
}

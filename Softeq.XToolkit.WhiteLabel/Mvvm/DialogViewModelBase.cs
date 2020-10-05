// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    /// <summary>
    ///     ViewModel for the modal dialogs.
    /// </summary>
    public class DialogViewModelBase : ViewModelBase, IDialogViewModel
    {
        protected DialogViewModelBase()
        {
            DialogComponent = new DialogViewModelComponent();
        }

        /// <summary>
        ///     Gets the instance of <see cref="T:Softeq.XToolkit.WhiteLabel.Mvvm.DialogViewModelComponent"/>
        ///     that contains all the required methods and properties
        ///     to close the dialog and track its state.
        /// </summary>
        public DialogViewModelComponent DialogComponent { get; }
    }
}

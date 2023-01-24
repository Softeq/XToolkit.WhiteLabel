// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Flows
{
    public class DissmissableDialogComponentViewModel : ViewModelBase, IDialogViewModel
    {
        public DissmissableDialogComponentViewModel()
        {
            DialogComponent = new DissmissableDialogViewModelComponent();
        }

        /// <summary>
        ///     Gets the instance of <see cref="DialogViewModelComponent"/>
        ///     that contains all the required methods and properties
        ///     to close the dialog and track its state.
        /// </summary>
        public DialogViewModelComponent DialogComponent { get; }
    }
}
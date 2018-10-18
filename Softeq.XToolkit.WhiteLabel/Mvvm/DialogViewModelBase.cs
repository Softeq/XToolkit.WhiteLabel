// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public class DialogViewModelBase : ViewModelBase, IDialogViewModel
    {
        private readonly Lazy<DialogViewModelComponent> _dialogComponentLazy;

        public DialogViewModelBase()
        {
            _dialogComponentLazy = new Lazy<DialogViewModelComponent>(() => new DialogViewModelComponent(this));
        }

        public DialogViewModelComponent DialogComponent => _dialogComponentLazy.Value;
    }
}
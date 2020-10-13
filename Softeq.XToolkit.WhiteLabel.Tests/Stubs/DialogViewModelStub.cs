// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Tests.Stubs
{
    public class DialogViewModelStub : ViewModelStub, IDialogViewModel
    {
        public DialogViewModelComponent DialogComponent => throw new NotImplementedException();
    }
}

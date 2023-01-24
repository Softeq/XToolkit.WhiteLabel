// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.Flows
{
    public class DissmissableDialogViewModelComponent : DialogViewModelComponent
    {
        public event EventHandler Dismissed;

        public void OnDismiss()
        {
            Dismissed?.Invoke(this, EventArgs.Empty);
        }
    }
}

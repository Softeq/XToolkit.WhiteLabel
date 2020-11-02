// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    public interface IDialogFragment
    {
        /// <summary>
        ///     Event called when Dialog fragment will dismiss.
        /// </summary>
        public event EventHandler? WillDismiss;
        
        /// <summary>
        ///     Event called when Dialog fragment is dismissed.
        /// </summary>
        public event EventHandler? Dismissed;

        /// <summary>
        ///     Show Dialog fragment.
        /// </summary>
        void Show();
    }
}

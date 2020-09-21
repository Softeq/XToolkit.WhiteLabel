// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using AndroidX.Activity;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class BackPressedCallback : OnBackPressedCallback
    {
        private Action? _handler;

        public BackPressedCallback(bool enabled, Action handler)
            : base(enabled)
        {
            _handler = handler;
        }

        public override void HandleOnBackPressed()
        {
            _handler?.Invoke();
        }

        public void Detach()
        {
            _handler = null;
            Remove();
        }
    }
}

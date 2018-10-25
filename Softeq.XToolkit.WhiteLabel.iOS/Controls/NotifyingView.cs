// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CoreGraphics;

namespace Softeq.XToolkit.WhiteLabel.iOS.Controls
{
    public abstract class NotifyingView : CustomViewBase, INotifyPropertyChanged
    {
        protected NotifyingView(IntPtr handle) : base(handle)
        {
        }

        protected NotifyingView(CGRect frame) : base(frame)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
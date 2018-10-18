// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CoreGraphics;

namespace Softeq.XToolkit.WhiteLabel.iOS.Controls
{
    public class NotifyingView : CustomViewBase, INotifyPropertyChanged
    {
        public NotifyingView(IntPtr handle) : base(handle)
        {
        }

        public NotifyingView(CGRect frame) : base(frame)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.Controls
{
    [Register("TextFieldWithoutPaste")]
    public class TextFieldWithoutPaste : UITextField
    {
        public TextFieldWithoutPaste(IntPtr handle) : base(handle)
        {
        }

        public override bool CanPerform(Selector action, NSObject withSender)
        {
            return false;
        }
    }
}
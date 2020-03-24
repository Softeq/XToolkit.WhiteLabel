// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Dialogs
{
    public class AlertAction
    {
        public readonly string Text;
        public readonly UIAlertActionStyle Style;
        public readonly Action Action;

        private AlertAction(string text, UIAlertActionStyle style, Action action)
        {
            Text = text;
            Style = style;
            Action = action;
        }

        public static AlertAction Default(string text, Action action)
        {
            return new AlertAction(text, UIAlertActionStyle.Default, action);
        }

        public static AlertAction Cancel(string text, Action action)
        {
            return new AlertAction(text, UIAlertActionStyle.Cancel, action);
        }

        public static AlertAction Destructive(string text, Action action)
        {
            return new AlertAction(text, UIAlertActionStyle.Destructive, action);
        }
    }
}

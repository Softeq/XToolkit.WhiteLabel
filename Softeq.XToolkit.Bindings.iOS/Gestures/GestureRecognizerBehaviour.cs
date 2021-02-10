// Developed by Softeq Development Corporation
// http://www.softeq.com


using Softeq.XToolkit.Common.Commands;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Gestures
{
    public abstract class GestureRecognizerBehaviour<T>
    {
        public ICommand<T>? Command { get; set; }

        protected void HandleGesture(T argument)
        {
            var command = Command;
            command?.Execute(argument);
        }

        protected void AddGestureRecognizer(UIView target, UIGestureRecognizer tap)
        {
            if (!target.UserInteractionEnabled)
            {
                target.UserInteractionEnabled = true;
            }

            target.AddGestureRecognizer(tap);
        }
    }
}

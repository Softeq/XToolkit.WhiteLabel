// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Gestures
{
    public abstract class GestureRecognizerBehaviour
    {
        public ICommand? Command { get; set; }

        protected void FireCommand(object? argument = null)
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

    public abstract class GestureRecognizerBehaviour<T>
        : GestureRecognizerBehaviour
    {
        protected virtual void HandleGesture(T gesture)
        {
        }
    }
}
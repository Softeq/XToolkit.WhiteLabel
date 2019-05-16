// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Gestures
{
    public abstract class GestureRecognizerBehavior
    {
        public ICommand Command { get; set; }

        protected void FireCommand(object argument = null)
        {
            var command = Command;
            command?.Execute(null);
        }

        protected void AddGestureRecognizer(UIView target, UIGestureRecognizer tap)
        {
            if (!target.UserInteractionEnabled)
                target.UserInteractionEnabled = true;

            target.AddGestureRecognizer(tap);
        }
    }

    public abstract class GestureRecognizerBehavior<T>
        : GestureRecognizerBehavior
    {
        protected virtual void HandleGesture(T gesture)
        {
        }
    }
}

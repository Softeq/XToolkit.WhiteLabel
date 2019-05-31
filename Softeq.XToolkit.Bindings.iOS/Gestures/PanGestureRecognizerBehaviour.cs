// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Gestures
{
    public class PanGestureRecognizerBehaviour
        : GestureRecognizerBehavior<UIPanGestureRecognizer>
    {
        protected override void HandleGesture(UIPanGestureRecognizer gesture)
        {
            FireCommand(gesture);
        }

        public PanGestureRecognizerBehaviour(UIView target)
        {
            var swipe = new UIPanGestureRecognizer(HandleGesture);

            AddGestureRecognizer(target, swipe);
        }
    }
}

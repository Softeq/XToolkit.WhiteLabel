// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Gestures
{
    public class PanGestureRecognizerBehaviour
        : GestureRecognizerBehaviour<UIPanGestureRecognizer>
    {
        public PanGestureRecognizerBehaviour(UIView target)
        {
            var swipe = new UIPanGestureRecognizer(HandleGesture);

            AddGestureRecognizer(target, swipe);
        }

        protected override void HandleGesture(UIPanGestureRecognizer gesture)
        {
            FireCommand(gesture);
        }
    }
}
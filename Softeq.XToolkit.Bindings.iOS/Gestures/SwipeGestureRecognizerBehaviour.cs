// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Gestures
{
    public class SwipeGestureRecognizerBehaviour
        : GestureRecognizerBehavior<UISwipeGestureRecognizer>
    {
        public SwipeGestureRecognizerBehaviour(UIView target, UISwipeGestureRecognizerDirection direction,
            uint numberOfTouchesRequired = 1)
        {
            var swipe = new UISwipeGestureRecognizer(HandleGesture)
            {
                Direction = direction,
                NumberOfTouchesRequired = numberOfTouchesRequired
            };

            AddGestureRecognizer(target, swipe);
        }

        protected override void HandleGesture(UISwipeGestureRecognizer gesture)
        {
            FireCommand();
        }
    }
}

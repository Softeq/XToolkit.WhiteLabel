// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Gestures
{
    public static class BehaviourExtensions
    {
        public static TapGestureRecognizerBehaviour Tap(this UIView view, uint numberOfTapsRequired = 1,
            uint numberOfTouchesRequired = 1,
            bool cancelsTouchesInView = true)
        {
            var toReturn = new TapGestureRecognizerBehaviour(view, numberOfTapsRequired, numberOfTouchesRequired,
                cancelsTouchesInView);
            return toReturn;
        }

        public static SwipeGestureRecognizerBehaviour Swipe(this UIView view,
            UISwipeGestureRecognizerDirection direction,
            uint numberOfTouchesRequired = 1)
        {
            var toReturn = new SwipeGestureRecognizerBehaviour(view, direction, numberOfTouchesRequired);
            return toReturn;
        }
    }
}

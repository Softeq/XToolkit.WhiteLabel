// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Gestures
{
    public class TapGestureRecognizerBehaviour
        : GestureRecognizerBehaviour<UITapGestureRecognizer>
    {
        public TapGestureRecognizerBehaviour(
            UIView target,
            uint numberOfTapsRequired = 1,
            uint numberOfTouchesRequired = 1,
            bool cancelsTouchesInView = true)
        {
            TapGestureRecogniser = new UITapGestureRecognizer(HandleGesture)
            {
                NumberOfTapsRequired = numberOfTapsRequired,
                NumberOfTouchesRequired = numberOfTouchesRequired,
                CancelsTouchesInView = cancelsTouchesInView
            };

            AddGestureRecognizer(target, TapGestureRecogniser);
        }

        public UITapGestureRecognizer TapGestureRecogniser { get; }

        protected override void HandleGesture(UITapGestureRecognizer gesture)
        {
            FireCommand(gesture);
        }
    }
}

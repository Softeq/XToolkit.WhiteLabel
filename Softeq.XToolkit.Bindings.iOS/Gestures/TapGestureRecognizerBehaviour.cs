// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Gestures
{
    public class TapGestureRecognizerBehaviour
        : GestureRecognizerBehavior<UITapGestureRecognizer>
    {
        public UITapGestureRecognizer TapGestureRecogniser { get; private set; }

        public TapGestureRecognizerBehaviour(UIView target, uint numberOfTapsRequired = 1,
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

        protected override void HandleGesture(UITapGestureRecognizer gesture)
        {
            FireCommand();
        }
    }
}

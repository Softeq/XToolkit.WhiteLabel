using System;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Navigation.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModalPresentation : Attribute
    {
        public readonly UIModalPresentationStyle ModalPresentationStyle;
        public readonly UIModalTransitionStyle ModalTransitionStyle;
        public readonly bool Animated;

        public ModalPresentation(UIModalPresentationStyle modalPresentationStyle,
            UIModalTransitionStyle modalTransitionStyle, bool animated = true)
        {
            ModalPresentationStyle = modalPresentationStyle;
            ModalTransitionStyle = modalTransitionStyle;
            Animated = animated;
        }
    }
}
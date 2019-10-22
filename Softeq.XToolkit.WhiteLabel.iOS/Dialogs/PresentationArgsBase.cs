using System;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Dialogs
{
    public abstract class PresentationArgsBase
    {
        public abstract UIModalPresentationStyle PresentationStyle { get; }

        public virtual UIViewControllerTransitioningDelegate TransitioningDelegate { get; }
    }
}

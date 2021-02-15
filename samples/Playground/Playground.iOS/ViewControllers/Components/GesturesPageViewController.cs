// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.Components;
using Softeq.XToolkit.Bindings.iOS.Gestures;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers.Components
{
    public partial class GesturesPageViewController : ViewControllerBase<GesturesPageViewModel>
    {
        public GesturesPageViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // add gesture handler directly
            var panCommand = new RelayCommand<UIPanGestureRecognizer>(_ => HighlightView(PanViewContainer));
            PanViewContainer.Pan().SetCommand(panCommand);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            // add gesture handler via bindings
            Bindings.Add(TapViewContainer.Tap()
                .Bind(_ => HighlightView(TapViewContainer)));

            Bindings.Add(SwipeViewContainer.Swipe(UISwipeGestureRecognizerDirection.Right)
                .Bind(_ => HighlightView(SwipeViewContainer)));
        }

        private void HighlightView(UIView view)
        {
            var originalColor = view.BackgroundColor;
            UIView.Animate(
                0.05,
                0,
                UIViewAnimationOptions.CurveLinear,
                () => view.BackgroundColor = UIColor.Red,
                () => view.BackgroundColor = originalColor);
        }
    }
}

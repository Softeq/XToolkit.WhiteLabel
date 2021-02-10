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

            var tapCommand = new RelayCommand<UITapGestureRecognizer>(args => HighlightView(TapViewContainer));
            TapViewContainer.Tap().Command = tapCommand;

            var swipeDirection = UISwipeGestureRecognizerDirection.Right;
            var swipeCommand = new RelayCommand<UISwipeGestureRecognizer>(args => HighlightView(SwipeViewContainer));
            SwipeViewContainer.Swipe(swipeDirection).Command = swipeCommand;

            var panCommand = new RelayCommand<UIPanGestureRecognizer>(args => HighlightView(PanViewContainer));
            PanViewContainer.Pan().Command = panCommand;
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

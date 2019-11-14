using System;
using System.Threading.Tasks;
using Playground.ViewModels.Components;
using Softeq.XToolkit.Bindings.iOS.Gestures;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers.Components
{
    public partial class GesturesPageViewController : ViewControllerBase<GesturesPageViewModel>
    {
        public GesturesPageViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TapViewContainer.Tap().Command = new AsyncCommand(async () =>
            {
               await HighlightView(TapViewContainer);
            });

            SwipeViewContainer.Swipe(UISwipeGestureRecognizerDirection.Right).Command = new AsyncCommand(async () =>
            {
                await HighlightView(SwipeViewContainer);
            });

            PanViewContainer.Pan().Command = new AsyncCommand(async () =>
            {
                await HighlightView(PanViewContainer);
            });
        }

        private async Task HighlightView (UIView view)
        {
            view.BackgroundColor = UIColor.Red;
            await Task.Delay(100);
            view.BackgroundColor = UIColor.White;
        }
    }
}


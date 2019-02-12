// Developed for PAWS-HALO by Softeq Development
// Corporation http://www.softeq.com

using System;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.ImagePicker
{
    public interface ICropper
    {
        void StartCropping(UIImage image, UIViewController viewController);
        event EventHandler<CropEventArgs> FinishedCropping;
        event EventHandler StartProcessing;
    }

    public class CropEventArgs : EventArgs
    {
        public CropEventArgs(UIImage image, bool isDismissed = false)
        {
            Image = image;
            IsDismissed = isDismissed;
        }

        public bool IsDismissed { get; }
        public UIImage Image { get; }
    }
}
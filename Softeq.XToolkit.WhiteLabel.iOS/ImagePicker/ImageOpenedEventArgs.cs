// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.ImagePicker
{
    public class ImageOpenedEventArgs
    {
        public string ImagePath { get; }
        public UIImage Thumbnail { get; }

        public ImageOpenedEventArgs(string imagePath, UIImage image)
        {
            ImagePath = imagePath;
            Thumbnail = image;
        }
    }
}

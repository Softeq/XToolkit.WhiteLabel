// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.ImagePicker;

namespace Softeq.XToolkit.WhiteLabel.iOS.ImagePicker
{
    public class ImagePickerOptions
    {
        public bool AllowEditing { get; set; }
        public ImagePickerOpenTypes ImagePickerOpenType { get; set; }
        public int ThumbnailWidth { get; }
        public int ThumbnailHeight { get; }

        public ImagePickerOptions(int thumbnailWidth, int thumbnailHeight)
        {
            ThumbnailWidth = thumbnailWidth;
            ThumbnailHeight = thumbnailHeight;
        }
    }
}
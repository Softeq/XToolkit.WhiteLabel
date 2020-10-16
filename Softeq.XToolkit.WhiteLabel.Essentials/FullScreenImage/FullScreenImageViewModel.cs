// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Essentials.FullScreenImage
{
    public class FullScreenImageViewModel : DialogViewModelBase
    {
        public string? ImageUrl => FullScreenImageOptions.ImageUrl;

        public string? ImagePath => FullScreenImageOptions.ImagePath;

        public FullScreenImageOptions FullScreenImageOptions { get; set; } = new FullScreenImageOptions();
    }
}

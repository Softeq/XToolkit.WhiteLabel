// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.ViewModels
{
    public class FullScreenImageOptions
    {
        public string? IosCloseButtonImageBundleName { get; set; }
        public int DroidCloseButtonImageResId { get; set; }
        public string? CloseButtonTintColor { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImagePath { get; set; }
    }

    public class FullScreenImageViewModel : DialogViewModelBase
    {
        public string? ImageUrl => FullScreenImageOptions.ImageUrl;
        public string? ImagePath => FullScreenImageOptions.ImagePath;

        public FullScreenImageOptions FullScreenImageOptions { get; set; } = default!;
    }
}
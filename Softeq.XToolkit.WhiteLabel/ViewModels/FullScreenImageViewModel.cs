// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.ViewModels
{
    public class FullScreenImageOptions
    {
        public string IosCloseButtonImageBoundleName { get; set; }
        public int DroidCloseButtonImageResId { get; set; }
        public string CloseButtonTintColor { get; set; }
        public string ImageUrl { get; set; }
        public string ImagePath { get; set; }
    }

    public class FullScreenImageViewModel : DialogViewModelBase, IViewModelParameter<FullScreenImageOptions>
    {
        public string ImageUrl => FullScreenImageOptions.ImageUrl;
        public string ImagePath => FullScreenImageOptions.ImagePath;

        public FullScreenImageOptions FullScreenImageOptions { get; set; }

        public FullScreenImageOptions Parameter
        {
            get => FullScreenImageOptions;
            set => FullScreenImageOptions = value;
        }
    }
}
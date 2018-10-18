// Developed by Softeq Development Corporation
// http://www.softeq.com

using FFImageLoading;
using FFImageLoading.Cross;
using FFImageLoading.Work;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Extensions
{
    public static class MvxCachedImageViewExtensions
    {
        public static void LoadImageAsync(this MvxCachedImageView imageView, string boundleResource, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                imageView.Image?.Dispose();
                imageView.Image = null;
                if (!string.IsNullOrEmpty(boundleResource))
                {
                    imageView.Image = UIImage.FromBundle(boundleResource);
                }

                return;
            }

            var expr = ImageService.Instance.LoadUrl(url);

            if (!string.IsNullOrEmpty(boundleResource))
            {
                expr = expr.LoadingPlaceholder(boundleResource, ImageSource.CompiledResource)
                    .ErrorPlaceholder(boundleResource, ImageSource.CompiledResource);
            }

            expr.IntoAsync(imageView);
        }
    }
}
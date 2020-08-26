// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Work;
using Softeq.XToolkit.WhiteLabel.Droid.Controls;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;

namespace Softeq.XToolkit.WhiteLabel.Droid.Extensions
{
    public static class ImageExtensions
    {
        public static void LoadImageWithTextPlaceholder(this ImageView imageView,
            string url,
            string name,
            AvatarPlaceholderDrawable.AvatarStyles styles,
            Action<TaskParameter>? transform = null)
        {
            var context = Dependencies.Container.Resolve<IContextProvider>().CurrentActivity;
            imageView.SetImageDrawable(new AvatarPlaceholderDrawable(context, name, styles));

            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            var loader = ImageService.Instance
                .LoadUrl(url)
                .DownSampleInDip(styles.Size.Width, styles.Size.Height);

            transform?.Invoke(loader);

            loader.IntoAsync(imageView);
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using CoreFoundation;
using Foundation;
using Softeq.XToolkit.WhiteLabel.iOS.Interfaces;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services;

/// <summary>
///     Default iOS implementation of <see cref="IIosImageService"/> based on iOS SDK.
/// </summary>
public class DefaultIosImageService : IIosImageService
{
    /// <inheritdoc />
    public void LoadImage(string url, UIImageView into)
    {
        DispatchQueue.DefaultGlobalQueue.DispatchAsync(() =>
        {
            using var nativeUrl = new NSUrl(url);
            using var data = NSData.FromUrl(nativeUrl);

            var image = UIImage.LoadFromData(data);
            if (image != null)
            {
                DispatchQueue.MainQueue.DispatchAsync(() =>
                {
                    into.Image = image;
                });
            }
        });
    }
}

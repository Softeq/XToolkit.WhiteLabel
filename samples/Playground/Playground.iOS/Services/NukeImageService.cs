// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using ImageCaching.Nuke;
using Softeq.XToolkit.WhiteLabel.iOS.Interfaces;
using UIKit;

namespace Playground.iOS.Services;

/// <summary>
///     Custom implementation of <see cref="IIosImageService"/> base on Nuke library.
/// </summary>
/// <remarks>
///     - Native library: https://github.com/kean/Nuke
///     - Xamarin binding: https://github.com/roubachof/NukeProxy.
/// </remarks>
public class NukeImageService : IIosImageService
{
    /// <inheritdoc />
    public void LoadImage(string url, UIImageView into)
    {
        using var nativeUrl = new NSUrl(url);

        ImagePipeline.Shared.LoadImageWithUrl(nativeUrl, (image, _) =>
        {
            into.Image = image;
        });
    }
}

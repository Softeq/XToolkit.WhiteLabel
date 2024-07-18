// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Interfaces;

/// <summary>
///     Provides methods to work with images on iOS.
/// </summary>
public interface IIosImageService
{
    /// <summary>
    ///     Load remote image <paramref name="into"/> target image view.
    /// </summary>
    /// <param name="url">Remote image url.</param>
    /// <param name="into">Target image view.</param>
    void LoadImage(string url, UIImageView into);
}

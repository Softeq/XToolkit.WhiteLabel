// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Widget;

namespace Softeq.XToolkit.WhiteLabel.Droid.Interfaces;

/// <summary>
///     Provides methods to work with images on Android.
/// </summary>
public interface IDroidImageService
{
    /// <summary>
    ///     Load remote image <paramref name="into"/> target image view.
    /// </summary>
    /// <param name="url">Remote image url.</param>
    /// <param name="into">Target image view.</param>
    void LoadImage(string url, ImageView into);
}

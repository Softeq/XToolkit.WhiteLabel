// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Widget;
using Bumptech.Glide;
using Softeq.XToolkit.WhiteLabel.Droid.Interfaces;

namespace Playground.Droid.Services;

/// <summary>
///     Custom implementation of <see cref="IDroidImageService"/> base on Glide library.
/// </summary>
/// <remarks>
///     - Native library: https://github.com/bumptech/glide
///     - Xamarin binding: https://github.com/xamarin/GooglePlayServicesComponents/tree/main/source/com.github.bumptech.glide.
/// </remarks>
public class GlideImageService : IDroidImageService
{
    /// <inheritdoc />
    public void LoadImage(string url, ImageView into)
    {
        Glide.With(into.Context).Load(url).Into(into);
    }
}

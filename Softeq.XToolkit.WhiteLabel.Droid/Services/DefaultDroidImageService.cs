// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Widget;
using Java.Net;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.Droid.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.Droid.Services;

/// <summary>
///     Default Android implementation of <see cref="IDroidImageService"/> based on Android SDK.
/// </summary>
public class DefaultDroidImageService : IDroidImageService
{
    /// <summary>
    ///     Gets internal in-memory cache.
    /// </summary>
    protected virtual SimpleLruCache<Drawable> ImageCache { get; } = new(size: 50);

    /// <inheritdoc />
    public void LoadImage(Context context, string url, ImageView into)
    {
        Task.Run(async () =>
        {
            var cachedDrawable = ImageCache.Get(url);
            if (cachedDrawable != null)
            {
                SetDrawable(into, cachedDrawable);
                return;
            }

            var downloadedDrawable = await DownloadDrawableAsync(url);
            if (downloadedDrawable == null)
            {
                return;
            }

            ImageCache.Put(url, downloadedDrawable);

            SetDrawable(into, downloadedDrawable);
        });
    }

    protected virtual async Task<Drawable?> DownloadDrawableAsync(string url)
    {
        var nativeUrl = new URL(url);
        var drawable = await Drawable.CreateFromStreamAsync(nativeUrl.OpenStream(), null);
        return drawable;
    }

    protected virtual void SetDrawable(ImageView image, Drawable drawable)
    {
        Execute.CurrentExecutor.OnUIThread(() =>
        {
            image.SetImageDrawable(drawable);
        });
    }

    /// <summary>
    ///     Simple In-Memory cache based on <see cref="T:Android.Util.LruCache" />.
    /// </summary>
    /// <typeparam name="T">Type of cache items.</typeparam>
    protected class SimpleLruCache<T> where T : Java.Lang.Object
    {
        private readonly LruCache _memoryCache;

        protected SimpleLruCache(LruCache cache)
        {
            _memoryCache = cache;
        }

        public SimpleLruCache(int size) : this(new LruCache(size))
        {
        }

        public void Put(string key, T drawable)
        {
            if (_memoryCache.Get(key) == null)
            {
                _memoryCache.Put(key, drawable);
            }
        }

        public T? Get(string key)
        {
            return _memoryCache.Get(key) as T;
        }
    }
}

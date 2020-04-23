// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Graphics;
using Android.Views;
using AndroidX.Core.Content;

namespace Softeq.XToolkit.WhiteLabel.Droid.Extensions
{
    public static class ViewExtensions
    {
        /// <summary>
        ///     Returns <see cref="Color" /> by resourceId.
        /// </summary>
        /// <param name="view">Android View.</param>
        /// <param name="resourceId">ResourceId of color</param>
        /// <returns>Color by resourceId.</returns>
        public static Color GetColor(this View view, int resourceId)
        {
            var intColor = ContextCompat.GetColor(view.Context, resourceId);
            return new Color(intColor);
        }
    }
}

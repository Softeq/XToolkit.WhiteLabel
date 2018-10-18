// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Graphics;
using Android.Support.V4.Content;
using Android.Views;

namespace Softeq.XToolkit.WhiteLabel.Droid.Extensions
{
    public static class ViewExtensions
    {
        public static Color GetColor(this View view, int resourceId)
        {
            var intColor = ContextCompat.GetColor(view.Context, resourceId);
            return new Color(intColor);
        }
    }
}
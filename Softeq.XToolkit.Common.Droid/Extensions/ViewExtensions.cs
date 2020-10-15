// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Views;

namespace Softeq.XToolkit.Common.Droid.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:Android.Views.View"/>.
    /// </summary>
    public static class ViewExtensions
    {
        /// <summary>
        ///     Remove target view from parent.
        /// </summary>
        /// <param name="view">Target view.</param>
        public static void RemoveFromParent(this View view)
        {
            (view.Parent as ViewGroup)?.RemoveView(view);
        }
    }
}

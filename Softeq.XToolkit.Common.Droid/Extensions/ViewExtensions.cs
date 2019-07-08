// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Views;

namespace Softeq.XToolkit.Common.Droid.Extensions
{
    /// <summary>
    /// Extensions related to <see cref="View" />
    /// </summary>
    public static class ViewExtensions
    {
        /// <summary>
        /// Remove target view from parent.
        /// </summary>
        /// <param name="view">Target view.</param>
        public static void RemoveFromParent(this View view)
        {
            ((ViewGroup) view?.Parent)?.RemoveView(view);
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.OS;
using Android.Views;

namespace Softeq.XToolkit.Common.Droid.Extensions
{
    /// <summary>
    ///     Extensions related to <see cref="T:Android.Views.View"/>.
    /// </summary>
    public static class ViewExtensions
    {
        /// <summary>
        ///     Remove target view from parent.
        /// </summary>
        /// <param name="view">Target view.</param>
        public static void RemoveFromParent(this View view)
        {
            ((ViewGroup) view.Parent)?.RemoveView(view);
        }

        /// <summary>
        ///     Executes provided action on UI thread.
        /// </summary>
        /// <param name="view">Instance of a <see cref="T:Android.Views.View"/>.</param>
        /// <param name="action">Action to be executed.</param>
        public static void BeginInvokeOnMainThread(this View view, Action action)
        {
            if (Looper.MainLooper.IsCurrentThread)
            {
                action.Invoke();
            }
            else
            {
                view.Post(action);
            }
        }
    }
}

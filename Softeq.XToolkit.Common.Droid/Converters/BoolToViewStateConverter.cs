// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Views;

namespace Softeq.XToolkit.Common.Droid.Converters
{
    public static class BoolToViewStateConverter
    {
        /// <summary>
        ///     if true ViewStates.Visible else ViewStates.Gone
        /// </summary>
        public static ViewStates ConvertGone(bool value)
        {
            return value ? ViewStates.Visible : ViewStates.Gone;
        }

        /// <summary>
        ///     if true ViewStates.Visible else ViewStates.Invisible
        /// </summary>
        public static ViewStates ConvertInvisible(bool value)
        {
            return value ? ViewStates.Visible : ViewStates.Invisible;
        }
    }
}
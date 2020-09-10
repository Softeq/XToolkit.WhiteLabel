// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Views;

namespace Softeq.XToolkit.Common.Droid.Converters
{
    /// <summary>
    ///     Converts <see cref="T:System.Boolean"/> to Android specific <see cref="T:Android.Views.ViewStates"/>.
    /// </summary>
    public static class BoolToViewStateConverter
    {
        public static GoneConverter GoneConverter { get; } = new GoneConverter();

        /// <summary>
        ///     Converts <see cref="T:System.Boolean"/> to <see cref="F:Android.Views.ViewStates.Gone"/>
        ///     or <see cref="F:Android.Views.ViewStates.Visible"/> depending on the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value"><see cref="T:System.Boolean"/> value to convert.</param>
        /// <returns>
        ///     <see cref="F:Android.Views.ViewStates.Visible"/> if <paramref name="value"/> is <see langword="true"/>,
        ///     <see cref="F:Android.Views.ViewStates.Gone"/> otherwise.
        /// </returns>
        public static ViewStates ConvertGone(bool value)
        {
            return value ? ViewStates.Visible : ViewStates.Gone;
        }

        /// <summary>
        ///     Converts <see cref="T:System.Boolean"/> to <see cref="F:Android.Views.ViewStates.Invisible"/>
        ///     or <see cref="F:Android.Views.ViewStates.Visible"/> depending on the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value"><see cref="T:System.Boolean"/> value to convert.</param>
        /// <returns>
        ///     <see cref="F:Android.Views.ViewStates.Visible"/> if <paramref name="value"/> is <see langword="true"/>,
        ///     <see cref="F:Android.Views.ViewStates.Invisible"/> otherwise.
        /// </returns>
        public static ViewStates ConvertInvisible(bool value)
        {
            return value ? ViewStates.Visible : ViewStates.Invisible;
        }
    }
}

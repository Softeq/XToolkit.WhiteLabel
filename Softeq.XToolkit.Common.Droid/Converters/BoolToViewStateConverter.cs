// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Views;
using Softeq.XToolkit.Common.Converters;

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

        public static GoneConverter CreateGoneConverter()
        {
            return new GoneConverter();
        }
    }

    public class GoneConverter : IConverter<ViewStates, bool>
    {
        public ViewStates ConvertValue(bool value, object? parameter = null, string? language = null)
        {
            return value ? ViewStates.Visible : ViewStates.Gone;
        }

        public bool ConvertValueBack(ViewStates value, object? parameter = null, string? language = null)
        {
            return value == ViewStates.Visible;
        }
    }
}

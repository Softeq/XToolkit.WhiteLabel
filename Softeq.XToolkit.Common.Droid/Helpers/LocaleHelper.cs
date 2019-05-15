// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content;

namespace Softeq.XToolkit.Common.Droid.Helpers
{
    public static class LocaleHelper
    {
        public static bool Is24HourFormat(Context context)
        {
            return Android.Text.Format.DateFormat.Is24HourFormat(context);
        }
    }
}

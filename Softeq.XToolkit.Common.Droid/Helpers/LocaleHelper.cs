// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content;
using Android.Text.Format;

namespace Softeq.XToolkit.Common.Droid.Helpers
{
    public static class LocaleHelper
    {
        public static bool Is24HourFormat(Context context)
        {
            return DateFormat.Is24HourFormat(context);
        }
    }
}

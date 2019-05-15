// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;

namespace Softeq.XToolkit.Common.iOS.Helpers
{
    public static class LocaleHelper
    {
        public static bool Is24HourFormat
        {
            get 
            {
                var template = NSDateFormatter.GetDateFormatFromTemplate("j", 0, NSLocale.CurrentLocale);
                return !template.Contains("a");
            }
        }
    }
}
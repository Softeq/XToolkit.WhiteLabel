// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;

namespace Softeq.XToolkit.Common.iOS.Extensions
{
    public static class NSLocaleExtensions
    {
        /// <summary>
        ///     Gets a value indicating whether <c>true</c> if times should be formatted as 24 hour times,
        ///     <c>false</c> if times should be formatted as 12 hour (AM/PM) times.
        ///
        ///     Based on the user's chosen locale and other preferences.
        /// </summary>
        public static bool IsCurrentLocale24HourFormat => Is24HourFormat(NSLocale.CurrentLocale);

        /// <summary>
        ///     Gets a value indicating whether <c>true</c> if times should be formatted as 24 hour times,
        ///     <c>false</c> if times should be formatted as 12 hour (AM/PM) times.
        /// </summary>
        /// <param name="locale">Locale to check.</param>
        /// <returns><c>true</c> if 24 hour time format is selected, <c>false</c> otherwise.</returns>
        public static bool Is24HourFormat(this NSLocale locale)
        {
            if (locale == null)
            {
                throw new ArgumentNullException(nameof(locale));
            }

            var template = NSDateFormatter.GetDateFormatFromTemplate("j", 0, locale);
            return !template.Contains("a");
        }
    }
}

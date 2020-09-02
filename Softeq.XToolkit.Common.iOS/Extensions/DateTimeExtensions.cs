// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;

namespace Softeq.XToolkit.Common.iOS.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts input <see cref="NSDate"/> to C# <see cref="DateTime"/>.
        /// </summary>
        /// <param name="date"><see cref="NSDate"/> object to convert.</param>
        /// <returns>
        /// <see cref="DateTime"/> representation of the specified date in UTC.
        /// No convertion is made since NSDate always represents date in UTC.
        /// </returns>
        public static DateTime ToUtcDateTime(this NSDate date)
        {
            return (DateTime) date;
        }

        /// <summary>
        /// Converts input <see cref="NSDate"/> to C# <see cref="DateTime"/>.
        /// </summary>
        /// <param name="date"><see cref="NSDate"/> object to convert.</param>
        /// <returns>
        /// <see cref="DateTime"/> representation of the specified <see cref="NSDate"/>
        /// whose <see cref="DateTime.Kind"/> property is <see cref="DateTimeKind.Local"/>
        /// and whose value is the local time equivalent to the specified <see cref="NSDate"/>.
        /// </returns>
        public static DateTime ToLocalDateTime(this NSDate date)
        {
            return ((DateTime) date).ToLocalTime();
        }

        /// <summary>
        /// Converts input <see cref="DateTime"/> to iOS specific <see cref="NSDate"/>.
        /// Since <see cref="NSDate"/> objects encapsulate a single point in time, independent of any particular calendrical system or time zone,
        /// we need to convert input date to <see cref="DateTimeKind.Utc"/>.
        /// </summary>
        /// <param name="date"><see cref="DateTime"/> object to convert.</param>
        /// <returns><see cref="NSDate"/> representation of the specified date converted to UTC.</returns>
        public static NSDate ToNsDate(this DateTime date)
        {
            return (NSDate) date.ToUniversalTime();
        }
    }
}

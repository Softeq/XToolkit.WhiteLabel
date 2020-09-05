// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;

namespace Softeq.XToolkit.Common.iOS.Extensions
{
    public static class NSDateExtensions
    {
        /// <summary>
        ///     Converts input <see cref="T:Foundation.NSDate"/> to C# <see cref="T:System.DateTime"/>.
        /// </summary>
        /// <param name="date"><see cref="T:Foundation.NSDate"/> object to convert.</param>
        /// <returns>
        /// <see cref="T:System.DateTime"/> representation of the specified date in UTC.
        /// No conversion is made since NSDate always represents date in UTC.
        /// </returns>
        public static DateTime ToUtcDateTime(this NSDate date)
        {
            return (DateTime) date;
        }

        /// <summary>
        ///     Converts input <see cref="T:Foundation.NSDate"/> to C# <see cref="T:System.DateTime"/>.
        /// </summary>
        /// <param name="date"><see cref="T:Foundation.NSDate"/> object to convert.</param>
        /// <returns>
        ///     <see cref="T:System.DateTime"/> representation of the specified <see cref="T:Foundation.NSDate"/>
        ///     whose <see cref="P:System.DateTime.Kind"/> property is <see cref="F:System.DateTimeKind.Local"/>
        ///     and whose value is the local time equivalent to the specified <see cref="T:Foundation.NSDate"/>.
        /// </returns>
        public static DateTime ToLocalDateTime(this NSDate date)
        {
            return ((DateTime) date).ToLocalTime();
        }

        /// <summary>
        ///     Converts input <see cref="T:System.DateTime"/> to iOS specific <see cref="T:Foundation.NSDate"/>.
        ///     Since <see cref="T:Foundation.NSDate"/> objects encapsulate a single point in time,
        ///     independent of any particular calendrical system or time zone,
        ///     we need to convert input date to <see cref="F:System.DateTimeKind.Utc"/>.
        /// </summary>
        /// <param name="date"><see cref="T:System.DateTime"/> object to convert.</param>
        /// <returns>
        ///     <see cref="T:Foundation.NSDate"/> representation of the specified date converted to UTC.
        /// </returns>
        public static NSDate ToNsDate(this DateTime date)
        {
            return (NSDate) date.ToUniversalTime();
        }
    }
}

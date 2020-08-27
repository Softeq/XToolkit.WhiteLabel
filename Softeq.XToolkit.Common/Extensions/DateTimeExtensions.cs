// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        ///     Standard amount of days in a week.
        /// </summary>
        public const int DaysInWeek = 7;

        /// <summary>
        ///     Finds the date of the first day in week of the given date.
        /// </summary>
        /// <param name="date">Date for which the start of the week is calculated.</param>
        /// <param name="startDayOfWeek">DayOfWeek value of the start of the week. Default is Sunday.</param>
        /// <returns>DateTime of the first day in required week.</returns>
        public static DateTime FirstDayOfWeek(this DateTime date, DayOfWeek startDayOfWeek = System.DayOfWeek.Sunday)
        {
            int diff = (DaysInWeek + (date.DayOfWeek - startDayOfWeek)) % DaysInWeek;
            return date.AddDays(-diff).Date;
        }

        /// <summary>
        ///     Finds the date of the last day in week of the given date.
        /// </summary>
        /// <param name="date">Date for which the end of the week is calculated.</param>
        /// <param name="endDayOfWeek">DayOfWeek value of the end of the week. Default is Saturday.</param>
        /// <returns>DateTime of the last day in required week.</returns>
        public static DateTime LastDayOfWeek(this DateTime date, DayOfWeek endDayOfWeek = System.DayOfWeek.Saturday)
        {
            int diff = (DaysInWeek + (endDayOfWeek - date.DayOfWeek)) % DaysInWeek;
            return date.AddDays(diff).Date;
        }

        /// <summary>
        ///     Finds the date of the first day in month of the given date.
        /// </summary>
        /// <param name="date">Date for which the start of the month is calculated.</param>
        /// <returns>DateTime of the first day in required month.</returns>
        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return date.AddDays(-date.Day + 1).Date;
        }

        /// <summary>
        ///     Finds the date of the last day in month of the given date.
        /// </summary>
        /// <param name="date">Date for which the end of the month is calculated.</param>
        /// <returns>DateTime of the last day in required month.</returns>
        public static DateTime LastDayOfMonth(this DateTime date)
        {
            var nextMonthDate = date.AddMonths(1);
            return nextMonthDate.AddDays(-nextMonthDate.Day).Date;
        }

        /// <summary>
        ///     Determines if the given date is today.
        /// </summary>
        /// <param name="date">Date to check if it is today.</param>
        /// <returns><c>true</c> if the date is today, <c>false</c> otherwise.</returns>
        public static bool IsToday(this DateTime date)
        {
            return DateTime.Today == date.Date;
        }

        /// <summary>
        ///     Determines if the given date was yesterday.
        /// </summary>
        /// <param name="date">Date to check if it was yesterday.</param>
        /// <returns><c>true</c> if the date was yesterday, <c>false</c> otherwise.</returns>
        public static bool IsYesterday(this DateTime date)
        {
            return (DateTime.Today - date.Date).Days == 1;
        }

        /// <summary>
        ///     Determines if the given date is included in current week. You can specify the start of the week.
        /// </summary>
        /// <param name="date">Date to check if in current week.</param>
        /// <param name="startDayOfWeek">DayOfWeek value of the start of the week. Default is Sunday.</param>
        /// <returns><c>true</c> if the date is in current week, <c>false</c> otherwise.</returns>
        public static bool IsInCurrentWeek(this DateTime date, DayOfWeek startDayOfWeek = System.DayOfWeek.Sunday)
        {
            var startOfWeek = DateTime.Today.FirstDayOfWeek(startDayOfWeek);
            return date.Date >= startOfWeek && date.Date < startOfWeek.AddDays(DaysInWeek);
        }

        /// <summary>
        ///     Finds the index of this date day in a week. You can specify the start of the week.
        /// </summary>
        /// <param name="date">Date to find the day number.</param>
        /// <param name="startDayOfWeek">DayOfWeek value of the start of the week. Default is Sunday.</param>
        /// <returns>The index of the specified day in the week.</returns>
        public static int DayOfWeek(this DateTime date, DayOfWeek startDayOfWeek = System.DayOfWeek.Sunday)
        {
            var dayOfWeek = ((int) date.DayOfWeek - (int) startDayOfWeek + DaysInWeek) % DaysInWeek;
            return dayOfWeek;
        }
    }
}

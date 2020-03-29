﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary> Standard amount of days in a week </summary>
        public const int DaysInWeek = 7;

        /// <summary>
        /// Finds the date of the first day in week of the given date
        /// </summary>
        /// <param name="date">Date for which the start of the week is calculated</param>
        /// <param name="startDayOfWeek">DayOfWeek value of the start of the week. Default is Sunday</param>
        /// <returns>DateTime of the first day in required week</returns>
        public static DateTime FirstDayOfWeek(this DateTime date, DayOfWeek startDayOfWeek = System.DayOfWeek.Sunday)
        {
            var firstDay = date;
            while (firstDay.DayOfWeek != startDayOfWeek)
            {
                firstDay = firstDay.AddDays(-1);
            }

            return firstDay;
        }

        /// <summary> Finds the date of the last day in week of the given date </summary>
        /// <param name="date">Date for which the end of the week is calculated</param>
        /// <param name="endDayOfWeek">DayOfWeek value of the end of the week. Default is Saturday</param>
        /// <returns>DateTime of the last day in required week</returns>
        public static DateTime LastDayOfWeek(this DateTime date, DayOfWeek endDayOfWeek = System.DayOfWeek.Saturday)
        {
            var lastDay = date;
            while (lastDay.DayOfWeek != endDayOfWeek)
            {
                lastDay = lastDay.AddDays(1);
            }

            return lastDay;
        }

        /// <summary> Determines if the given date is today </summary>
        /// <param name="date">Date to check if it is today</param>
        /// <returns><c>true</c> if the date is today, <c>false</c> otherwise</returns>
        public static bool IsToday(this DateTime date)
        {
            return DateTime.Today == date.Date;
        }

        /// <summary> Determines if the given date was yesterday </summary>
        /// <param name="date">Date to check if it was yesterday</param>
        /// <returns><c>true</c> if the date was yesterday, <c>false</c> otherwise</returns>
        public static bool IsYesterday(this DateTime date)
        {
            return (DateTime.Today - date.Date).Days == 1;
        }

        // TODO: add unit tests for methods below
        /// <summary>
        ///     Determines if the given date is included in current week. You can specify the start and the length of the week
        /// </summary>
        /// <param name="date">Date to check if in current week</param>
        /// <param name="startDayOfWeek">DayOfWeek value of the start of the week. Default is Sunday</param>
        /// <param name="weekLength">Length of the week. Default is 7</param>
        /// <returns><c>true</c> if the date is in current week, <c>false</c> otherwise</returns>
        public static bool IsInCurrentWeek(this DateTime date, DayOfWeek startDayOfWeek = System.DayOfWeek.Sunday,
            int weekLength = DaysInWeek)
        {
            var startOfWeek = DateTime.Today.FirstDayOfWeek(startDayOfWeek);
            return date.Date >= startOfWeek && date.Date < startOfWeek.AddDays(weekLength);
        }

        /// <summary>
        ///     Finds the number of this date day in a week. You can specify the start and the length of the week
        /// </summary>
        /// <param name="date">Date to find the day number</param>
        /// <param name="startDayOfWeek">DayOfWeek value of the start of the week. Default is Sunday</param>
        /// <param name="weekLength">Length of the week. Default is 7</param>
        /// <returns></returns>
        public static int DayOfWeek(this DateTime date, DayOfWeek startDayOfWeek = System.DayOfWeek.Sunday,
            int weekLength = DaysInWeek)
        {
            var dayOfWeek = ((int) date.DayOfWeek - (int) startDayOfWeek + weekLength) % weekLength;

            return dayOfWeek;
        }
    }
}

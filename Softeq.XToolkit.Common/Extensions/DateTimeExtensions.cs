// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime FirstDayOfWeek(this DateTime date)
        {
            var firstDay = date;
            while (firstDay.DayOfWeek != System.DayOfWeek.Monday)
            {
                firstDay = firstDay.AddDays(-1);
            }

            return firstDay;
        }

        public static DateTime LastDayOfWeek(this DateTime date)
        {
            var lastDay = date;
            while (lastDay.DayOfWeek != System.DayOfWeek.Sunday)
            {
                lastDay = lastDay.AddDays(1);
            }

            return lastDay;
        }

        public static bool IsToday(this DateTime date)
        {
            return DateTime.Today == date.Date;
        }

        public static bool IsYesterday(this DateTime date)
        {
            return (DateTime.Today - date.Date).Days == 1;
        }

        public static bool IsInCurrentWeek(this DateTime date)
        {
            return date.Date >= DateTime.Today.FirstDayOfWeek() && date.Date <= DateTime.Today.LastDayOfWeek();
        }

        public static int DayOfWeek(this DateTime date)
        {
            var dayOfWeek = (int) date.DayOfWeek;
            if (dayOfWeek == 0)
            {
                dayOfWeek = 6;
            }

            return dayOfWeek;
        }

        public static bool IsLaterThan(this DateTime date, DateTime value)
        {
            return date.CompareTo(value) > 0;
        }

        public static bool IsEarlierThan(this DateTime date, DateTime value)
        {
            return date.CompareTo(value) < 0;
        }

        public static bool IsEqual(this DateTime date, DateTime value)
        {
            return date.CompareTo(value) == 0;
        }

        public static bool IsLaterOrEqualThan(this DateTime date, DateTime value)
        {
            return date.CompareTo(value) >= 0;
        }

        public static bool IsEarlierOrEqualThan(this DateTime date, DateTime value)
        {
            return date.CompareTo(value) <= 0;
        }
    }
}
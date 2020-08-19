// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;

namespace Softeq.XToolkit.Common.Tests.Extensions.DateTimeExtensionsTests
{
    public class DateTimeExtensionsDataProvider
    {
        public static IEnumerable<object[]> SundayStartWeekData
        {
            get
            {
                yield return new object[] { new DateTime(2019, 12, 28), new DateTime(2019, 12, 22) }; // Sa prev. week
                yield return new object[] { new DateTime(2019, 12, 29), new DateTime(2019, 12, 29) }; // Su curr. week
                yield return new object[] { new DateTime(2019, 12, 30), new DateTime(2019, 12, 29) }; // Mo curr. week
                yield return new object[] { new DateTime(2019, 12, 31), new DateTime(2019, 12, 29) }; // Tu curr. week
                yield return new object[] { new DateTime(2020, 1, 1), new DateTime(2019, 12, 29) }; // We curr. week
                yield return new object[] { new DateTime(2020, 1, 2), new DateTime(2019, 12, 29) }; // Th curr. week
                yield return new object[] { new DateTime(2020, 1, 3), new DateTime(2019, 12, 29) }; // Fr curr. week
                yield return new object[] { new DateTime(2020, 1, 4), new DateTime(2019, 12, 29) }; // Sa curr. week
                yield return new object[] { new DateTime(2020, 1, 5), new DateTime(2020, 1, 5) }; // Su next week
            }
        }

        public static IEnumerable<object[]> MondayStartWeekData
        {
            get
            {
                yield return new object[] { new DateTime(2019, 12, 29), new DateTime(2019, 12, 23) }; // Su prev. week
                yield return new object[] { new DateTime(2019, 12, 30), new DateTime(2019, 12, 30) }; // Mo curr. week
                yield return new object[] { new DateTime(2019, 12, 31), new DateTime(2019, 12, 30) }; // Tu curr. week
                yield return new object[] { new DateTime(2020, 1, 1), new DateTime(2019, 12, 30) }; // We curr. week
                yield return new object[] { new DateTime(2020, 1, 2), new DateTime(2019, 12, 30) }; // Th curr. week
                yield return new object[] { new DateTime(2020, 1, 3), new DateTime(2019, 12, 30) }; // Fr curr. week
                yield return new object[] { new DateTime(2020, 1, 4), new DateTime(2019, 12, 30) }; // Sa curr. week
                yield return new object[] { new DateTime(2020, 1, 5), new DateTime(2019, 12, 30) }; // Su curr. week
                yield return new object[] { new DateTime(2020, 1, 6), new DateTime(2020, 1, 6) }; // Mo next week
            }
        }

        public static IEnumerable<object[]> SaturdayEndWeekData
        {
            get
            {
                yield return new object[] { new DateTime(2019, 12, 28), new DateTime(2019, 12, 28) }; // Sa prev. week
                yield return new object[] { new DateTime(2019, 12, 29), new DateTime(2020, 1, 4) }; // Su curr. week
                yield return new object[] { new DateTime(2019, 12, 30), new DateTime(2020, 1, 4) }; // Mo curr. week
                yield return new object[] { new DateTime(2019, 12, 31), new DateTime(2020, 1, 4) }; // Tu curr. week
                yield return new object[] { new DateTime(2020, 1, 1), new DateTime(2020, 1, 4) }; // We curr. week
                yield return new object[] { new DateTime(2020, 1, 2), new DateTime(2020, 1, 4) }; // Th curr. week
                yield return new object[] { new DateTime(2020, 1, 3), new DateTime(2020, 1, 4) }; // Fr curr. week
                yield return new object[] { new DateTime(2020, 1, 4), new DateTime(2020, 1, 4) }; // Sa curr. week
                yield return new object[] { new DateTime(2020, 1, 5), new DateTime(2020, 1, 11) }; // Su next week
            }
        }

        public static IEnumerable<object[]> SundayEndWeekData
        {
            get
            {
                yield return new object[] { new DateTime(2019, 12, 29), new DateTime(2019, 12, 29) }; // Su prev. week
                yield return new object[] { new DateTime(2019, 12, 30), new DateTime(2020, 1, 5) }; // Mo curr. week
                yield return new object[] { new DateTime(2019, 12, 31), new DateTime(2020, 1, 5) }; // Tu curr. week
                yield return new object[] { new DateTime(2020, 1, 1), new DateTime(2020, 1, 5) }; // We curr. week
                yield return new object[] { new DateTime(2020, 1, 2), new DateTime(2020, 1, 5) }; // Th curr. week
                yield return new object[] { new DateTime(2020, 1, 3), new DateTime(2020, 1, 5) }; // Fr curr. week
                yield return new object[] { new DateTime(2020, 1, 4), new DateTime(2020, 1, 5) }; // Sa curr. week
                yield return new object[] { new DateTime(2020, 1, 5), new DateTime(2020, 1, 5) }; // Su curr. week
                yield return new object[] { new DateTime(2020, 1, 6), new DateTime(2020, 1, 12) }; // Mo next week
            }
        }

        public static IEnumerable<object[]> FirstDayOfMonthData
        {
            get
            {
                yield return new object[] { new DateTime(2019, 1, 15), new DateTime(2019, 1, 1) };
                yield return new object[] { new DateTime(2019, 2, 28), new DateTime(2019, 2, 1) };
                yield return new object[] { new DateTime(2019, 3, 8), new DateTime(2019, 3, 1) };
                yield return new object[] { new DateTime(2020, 4, 2), new DateTime(2020, 4, 1) };
                yield return new object[] { new DateTime(2020, 5, 1), new DateTime(2020, 5, 1) };
                yield return new object[] { new DateTime(2020, 6, 20), new DateTime(2020, 6, 1) };
                yield return new object[] { new DateTime(2021, 7, 11), new DateTime(2021, 7, 1) };
                yield return new object[] { new DateTime(2021, 8, 31), new DateTime(2021, 8, 1) };
                yield return new object[] { new DateTime(2021, 9, 13), new DateTime(2021, 9, 1) };
                yield return new object[] { new DateTime(1925, 10, 6), new DateTime(1925, 10, 1) };
                yield return new object[] { new DateTime(1925, 11, 24), new DateTime(1925, 11, 1) };
                yield return new object[] { new DateTime(1925, 12, 18), new DateTime(1925, 12, 1) };
            }
        }

        public static IEnumerable<object[]> LastDayOfMonthData
        {
            get
            {
                yield return new object[] { new DateTime(2019, 1, 15), new DateTime(2019, 1, 31) };
                yield return new object[] { new DateTime(2019, 2, 28), new DateTime(2019, 2, 28) };
                yield return new object[] { new DateTime(2019, 3, 8), new DateTime(2019, 3, 31) };
                yield return new object[] { new DateTime(2020, 4, 2), new DateTime(2020, 4, 30) };
                yield return new object[] { new DateTime(2020, 5, 1), new DateTime(2020, 5, 31) };
                yield return new object[] { new DateTime(2020, 6, 20), new DateTime(2020, 6, 30) };
                yield return new object[] { new DateTime(2021, 7, 11), new DateTime(2021, 7, 31) };
                yield return new object[] { new DateTime(2021, 8, 31), new DateTime(2021, 8, 31) };
                yield return new object[] { new DateTime(2021, 9, 13), new DateTime(2021, 9, 30) };
                yield return new object[] { new DateTime(1925, 10, 6), new DateTime(1925, 10, 31) };
                yield return new object[] { new DateTime(1925, 11, 24), new DateTime(1925, 11, 30) };
                yield return new object[] { new DateTime(1925, 12, 18), new DateTime(1925, 12, 31) };
            }
        }

        public static IEnumerable<object[]> TodayCheckData
        {
            get
            {
                yield return new object[] { DateTime.Today, true }; // start of Today
                yield return new object[] { DateTime.Today.AddHours(12), true }; // middle of Today
                yield return new object[] { DateTime.Today.AddHours(24).AddTicks(-1), true }; // end of Today
                yield return new object[] { DateTime.Today.AddHours(24), false }; // start of Tomorrow
                yield return new object[] { DateTime.Today.AddTicks(-1), false }; // end of Yesterday
                yield return new object[] { DateTime.Today.AddDays(-1), false }; // one day before Today
                yield return new object[] { DateTime.Today.AddYears(-1), false }; // one year before Today
            }
        }

        public static IEnumerable<object[]> YesterdayCheckData
        {
            get
            {
                yield return new object[] { DateTime.Today, false }; // start of Today
                yield return new object[] { DateTime.Today.AddTicks(-1), true }; // end of Yesterday
                yield return new object[] { DateTime.Today.AddHours(-12), true }; // middle of Yesterday
                yield return new object[] { DateTime.Today.AddDays(-1), true }; // start of Yesterday
                yield return new object[] { DateTime.Today.AddDays(-1).AddTicks(-1), false }; // end of the day before Yesterday
            }
        }

        public static IEnumerable<object[]> InsideCurrentWeekData
        {
            get
            {
                yield return new object[] { DateTime.Today };
                yield return new object[] { DateTime.Today.AddDays(-(int) DateTime.Today.DayOfWeek) };
                yield return new object[] { DateTime.Today.AddDays(-(int) DateTime.Today.DayOfWeek + 6) };
            }
        }

        public static IEnumerable<object[]> InsideCurrentWeekWithStartDayData
        {
            get
            {
                yield return new object[] { DateTime.Today, DayOfWeek.Monday };
                yield return new object[]
                {
                    DateTime.Today.AddDays(-(int) DateTime.Today.DayOfWeek + (int) DayOfWeek.Tuesday
                        + (DateTime.Today.DayOfWeek < DayOfWeek.Tuesday ? -7 : 0)),
                    DayOfWeek.Tuesday
                };
                yield return new object[]
                {
                    DateTime.Today.AddDays(-(int) DateTime.Today.DayOfWeek + (int) DayOfWeek.Wednesday
                        + (DateTime.Today.DayOfWeek < DayOfWeek.Wednesday ? -1 : 6)),
                    DayOfWeek.Wednesday
                };
            }
        }

        public static IEnumerable<object[]> OutsideCurrentWeekData
        {
            get
            {
                yield return new object[] { DateTime.Today.AddDays(-(int) DateTime.Today.DayOfWeek - 1) };
                yield return new object[] { DateTime.Today.AddDays(-10) };
                yield return new object[] { DateTime.Today.AddMonths(-2) };
                yield return new object[] { DateTime.Today.AddYears(-1) };
                yield return new object[] { DateTime.Today.AddDays(-(int) DateTime.Today.DayOfWeek + 7) };
                yield return new object[] { DateTime.Today.AddDays(10) };
                yield return new object[] { DateTime.Today.AddMonths(2) };
                yield return new object[] { DateTime.Today.AddYears(1) };
            }
        }

        public static IEnumerable<object[]> OutsideCurrentWeekWithStartDayData
        {
            get
            {
                yield return new object[]
                {
                    DateTime.Today.AddDays(-(int) DateTime.Today.DayOfWeek + (int) DayOfWeek.Monday - 1
                        + (DateTime.Today.DayOfWeek < DayOfWeek.Monday ? -7 : 0)),
                    DayOfWeek.Monday
                };
                yield return new object[] { DateTime.Today.AddDays(-10), DayOfWeek.Tuesday };
                yield return new object[] { DateTime.Today.AddMonths(-2), DayOfWeek.Wednesday };
                yield return new object[] { DateTime.Today.AddYears(-1), DayOfWeek.Thursday };
                yield return new object[]
                {
                    DateTime.Today.AddDays(-(int) DateTime.Today.DayOfWeek + (int) DayOfWeek.Friday
                        + (DateTime.Today.DayOfWeek < DayOfWeek.Friday ? 0 : 7)),
                    DayOfWeek.Friday
                };
                yield return new object[] { DateTime.Today.AddDays(10), DayOfWeek.Saturday };
                yield return new object[] { DateTime.Today.AddMonths(2), DayOfWeek.Sunday };
                yield return new object[] { DateTime.Today.AddYears(1), DayOfWeek.Monday };
            }
        }

        public static IEnumerable<object[]> SundayDayOfWeekData
        {
            get
            {
                yield return new object[] { new DateTime(2019, 12, 29), 0 }; // Su
                yield return new object[] { new DateTime(2019, 12, 30), 1 }; // Mo
                yield return new object[] { new DateTime(2019, 12, 31), 2 }; // Tu
                yield return new object[] { new DateTime(2020, 1, 1), 3 }; // We
                yield return new object[] { new DateTime(2020, 1, 2), 4 }; // Th
                yield return new object[] { new DateTime(2020, 1, 3), 5 }; // Fr
                yield return new object[] { new DateTime(2020, 1, 4), 6 }; // Sa
                yield return new object[] { new DateTime(2020, 1, 5), 0 }; // Su
            }
        }

        public static IEnumerable<object[]> MondayDayOfWeekData
        {
            get
            {
                yield return new object[] { new DateTime(2019, 12, 29), 6 }; // Su
                yield return new object[] { new DateTime(2019, 12, 30), 0 }; // Mo
                yield return new object[] { new DateTime(2019, 12, 31), 1 }; // Tu
                yield return new object[] { new DateTime(2020, 1, 1), 2 }; // We
                yield return new object[] { new DateTime(2020, 1, 2), 3 }; // Th
                yield return new object[] { new DateTime(2020, 1, 3), 4 }; // Fr
                yield return new object[] { new DateTime(2020, 1, 4), 5 }; // Sa
                yield return new object[] { new DateTime(2020, 1, 5), 6 }; // Su
            }
        }
    }
}

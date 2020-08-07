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
    }
}

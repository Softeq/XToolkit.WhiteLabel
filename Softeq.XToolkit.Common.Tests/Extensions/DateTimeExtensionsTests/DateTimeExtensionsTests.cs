﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Extensions;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Extensions.DateTimeExtensionsTests
{
    public class DateTimeExtensionsTests
    {
        [Theory]
        [MemberData(nameof(DateTimeExtensionsDataProvider.SundayStartWeekData), MemberType = typeof(DateTimeExtensionsDataProvider))]
        public void FirstDayOfWeek_WithoutSpecifiedStartDay_ReturnsCorrectDate(DateTime testDate, DateTime expectedDate)
        {
            var firstDayOfWeek = testDate.FirstDayOfWeek();

            Assert.Equal(expectedDate, firstDayOfWeek);
        }

        [Theory]
        [MemberData(nameof(DateTimeExtensionsDataProvider.MondayStartWeekData), MemberType = typeof(DateTimeExtensionsDataProvider))]
        public void FirstDayOfWeek_WithSpecifiedStartDay_ReturnsCorrectDate(DateTime testDate, DateTime expectedDate)
        {
            var firstDayOfWeek = testDate.FirstDayOfWeek(DayOfWeek.Monday);

            Assert.Equal(expectedDate, firstDayOfWeek);
        }

        [Theory]
        [MemberData(nameof(DateTimeExtensionsDataProvider.SaturdayEndWeekData), MemberType = typeof(DateTimeExtensionsDataProvider))]
        public void LastDayOfWeek_WithoutSpecifiedStartDay_ReturnsCorrectDate(DateTime testDate, DateTime expectedDate)
        {
            var lastDayOfWeek = testDate.LastDayOfWeek();

            Assert.Equal(expectedDate, lastDayOfWeek);
        }

        [Theory]
        [MemberData(nameof(DateTimeExtensionsDataProvider.SundayEndWeekData), MemberType = typeof(DateTimeExtensionsDataProvider))]
        public void LastDayOfWeek_WithSpecifiedStartDay_ReturnsCorrectDate(DateTime testDate, DateTime expectedDate)
        {
            var lastDayOfWeek = testDate.LastDayOfWeek(DayOfWeek.Sunday);

            Assert.Equal(expectedDate, lastDayOfWeek);
        }

        [Theory]
        [MemberData(nameof(DateTimeExtensionsDataProvider.FirstDayOfMonthData), MemberType = typeof(DateTimeExtensionsDataProvider))]
        public void FirstDayOfMonth_ForDateTime_ReturnsCorrectDate(DateTime testDate, DateTime expectedDate)
        {
            var firstDayOfMonth = testDate.FirstDayOfMonth();

            Assert.Equal(expectedDate, firstDayOfMonth);
        }

        [Theory]
        [MemberData(nameof(DateTimeExtensionsDataProvider.LastDayOfMonthData), MemberType = typeof(DateTimeExtensionsDataProvider))]
        public void LastDayOfMonth_ForDateTime_ReturnsCorrectDate(DateTime testDate, DateTime expectedDate)
        {
            var lastDayOfMonth = testDate.LastDayOfMonth();

            Assert.Equal(expectedDate, lastDayOfMonth);
        }

        [Theory]
        [MemberData(nameof(DateTimeExtensionsDataProvider.TodayCheckData), MemberType = typeof(DateTimeExtensionsDataProvider))]
        public void IsToday_ForDateTime_ReturnsExpectedValue(DateTime testDate, bool expectedIsToday)
        {
            var isToday = testDate.IsToday();

            Assert.Equal(expectedIsToday, isToday);
        }

        [Theory]
        [MemberData(nameof(DateTimeExtensionsDataProvider.YesterdayCheckData), MemberType = typeof(DateTimeExtensionsDataProvider))]
        public void IsYesterday_ForDateTime_ReturnsExpectedValue(DateTime testDate, bool expectedIsYesterday)
        {
            var isYesterday = testDate.IsYesterday();

            Assert.Equal(expectedIsYesterday, isYesterday);
        }

        [Theory]
        [MemberData(nameof(DateTimeExtensionsDataProvider.InsideCurrentWeekData), MemberType = typeof(DateTimeExtensionsDataProvider))]
        public void IsInCurrentWeek_ForInsideCurrentWeekDays_ReturnsTrue(DateTime testDate)
        {
            var isInCurrentWeek = testDate.IsInCurrentWeek();
            Assert.True(isInCurrentWeek);
        }

        [Theory]
        [MemberData(nameof(DateTimeExtensionsDataProvider.InsideCurrentWeekWithStartDayData), MemberType = typeof(DateTimeExtensionsDataProvider))]
        public void IsInCurrentWeek_ForOInsideCurrentWeekDaysWithSpecifiedStartDay_ReturnsTrue(DateTime testDate, DayOfWeek startDay)
        {
            var isInCurrentWeek = testDate.IsInCurrentWeek(startDay);
            Assert.True(isInCurrentWeek);
        }

        [Theory]
        [MemberData(nameof(DateTimeExtensionsDataProvider.OutsideCurrentWeekData), MemberType = typeof(DateTimeExtensionsDataProvider))]
        public void IsInCurrentWeek_ForOutsideCurrentWeekDays_ReturnsFalse(DateTime testDate)
        {
            var isInCurrentWeek = testDate.IsInCurrentWeek();
            Assert.False(isInCurrentWeek);
        }

        [Theory]
        [MemberData(nameof(DateTimeExtensionsDataProvider.OutsideCurrentWeekWithStartDayData), MemberType = typeof(DateTimeExtensionsDataProvider))]
        public void IsInCurrentWeek_ForOutsideCurrentWeekDaysWithSpecifiedStartDay_ReturnsFalse(DateTime testDate, DayOfWeek startDay)
        {
            var isInCurrentWeek = testDate.IsInCurrentWeek(startDay);
            Assert.False(isInCurrentWeek);
        }

        [Theory]
        [MemberData(nameof(DateTimeExtensionsDataProvider.SundayDayOfWeekData), MemberType = typeof(DateTimeExtensionsDataProvider))]
        public void DayOfWeek_WithoutSpecifiedStartDay_ReturnsCorrectValue(DateTime testDate, int expectedValue)
        {
            var dayOfWeek = testDate.DayOfWeek();

            Assert.Equal(expectedValue, dayOfWeek);
        }

        [Theory]
        [MemberData(nameof(DateTimeExtensionsDataProvider.MondayDayOfWeekData), MemberType = typeof(DateTimeExtensionsDataProvider))]
        public void DayOfWeek_WithSpecifiedStartDay_ReturnsCorrectValue(DateTime testDate, int expectedValue)
        {
            var dayOfWeek = testDate.DayOfWeek(DayOfWeek.Monday);

            Assert.Equal(expectedValue, dayOfWeek);
        }
    }
}

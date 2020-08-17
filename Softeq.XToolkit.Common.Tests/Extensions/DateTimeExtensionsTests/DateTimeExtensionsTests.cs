// Developed by Softeq Development Corporation
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

        [Fact]
        public void IsInCurrentWeek_ForToday_ReturnsTrue()
        {
            var todayIsInCurrentWeek = DateTime.Today.IsInCurrentWeek();
            Assert.True(todayIsInCurrentWeek);

            var todayIsInCurrentMondayWeek = DateTime.Today.IsInCurrentWeek(DayOfWeek.Monday);
            Assert.True(todayIsInCurrentMondayWeek);
        }

        [Fact]
        public void IsInCurrentWeek_ForPreviousAndNextDaysOfToday_ReturnsTrueCorrectAmountOfTimes()
        {
            var weekLength = 7;

            var date = DateTime.Today;
            var isInCurrentWeek = date.IsInCurrentWeek();
            var count = isInCurrentWeek ? 1 : 0;

            for (var i = 1; isInCurrentWeek; i++)
            {
                var previousDay = date.AddDays(-i);
                var previousDayIsInCurrentWeek = previousDay.IsInCurrentWeek();
                var nextDay = date.AddDays(i);
                var nextDayIsInCurrentWeek = nextDay.IsInCurrentWeek();
                count += previousDayIsInCurrentWeek ? 1 : 0;
                count += nextDayIsInCurrentWeek ? 1 : 0;
                isInCurrentWeek = previousDayIsInCurrentWeek || nextDayIsInCurrentWeek;
            }

            Assert.Equal(weekLength, count);
        }

        [Theory]
        [MemberData(nameof(DateTimeExtensionsDataProvider.OutsideCurrentWeekData), MemberType = typeof(DateTimeExtensionsDataProvider))]
        public void IsInCurrentWeek_ForOutsideCurrentWeekDays_ReturnsFalse(DateTime testDate)
        {
            var isInCurrentWeek = testDate.IsInCurrentWeek();
            Assert.False(isInCurrentWeek);

            var isInCurrentMondayWeek = testDate.IsInCurrentWeek(DayOfWeek.Monday);
            Assert.False(isInCurrentMondayWeek);
        }

        [Fact]
        public void IsInCurrentWeek_ForBoundaryWeekDaysWithDefaultParam_ReturnsCorrectValues()
        {
            var firstWeekDay = DateTime.Today.FirstDayOfWeek();
            var firstDayIsInCurrentWeek = firstWeekDay.IsInCurrentWeek();
            Assert.True(firstDayIsInCurrentWeek);

            var previousToFirstWeekDay = firstWeekDay.AddDays(-1);
            var previousToFirstDayIsInCurrentWeek = previousToFirstWeekDay.IsInCurrentWeek();
            Assert.False(previousToFirstDayIsInCurrentWeek);

            var lastWeekDay = DateTime.Today.LastDayOfWeek();
            var lastDayIsInCurrentWeek = lastWeekDay.IsInCurrentWeek();
            Assert.True(lastDayIsInCurrentWeek);

            var nextToLastWeekDay = lastWeekDay.AddDays(1);
            var nextToLastDayIsInCurrentWeek = nextToLastWeekDay.IsInCurrentWeek();
            Assert.False(nextToLastDayIsInCurrentWeek);
        }

        [Theory]
        [InlineData(DayOfWeek.Monday, DayOfWeek.Sunday)]
        [InlineData(DayOfWeek.Wednesday, DayOfWeek.Tuesday)]
        [InlineData(DayOfWeek.Saturday, DayOfWeek.Friday)]
        public void IsInCurrentWeek_ForBoundaryWeekDaysWithSpecifiedParam_ReturnsCorrectValues(DayOfWeek startDayOfWeek, DayOfWeek endDayOfWeek)
        {
            var firstWeekDay = DateTime.Today.FirstDayOfWeek(startDayOfWeek);
            var firstDayIsInCurrentWeek = firstWeekDay.IsInCurrentWeek(startDayOfWeek);
            Assert.True(firstDayIsInCurrentWeek);

            var previousToFirstWeekDay = firstWeekDay.AddDays(-1);
            var previousToFirstDayIsInCurrentWeek = previousToFirstWeekDay.IsInCurrentWeek(startDayOfWeek);
            Assert.False(previousToFirstDayIsInCurrentWeek);

            var lastWeekDay = DateTime.Today.LastDayOfWeek(endDayOfWeek);
            var lastDayIsInCurrentWeek = lastWeekDay.IsInCurrentWeek(startDayOfWeek);
            Assert.True(lastDayIsInCurrentWeek);

            var nextToLastWeekDay = lastWeekDay.AddDays(1);
            var nextToLastDayIsInCurrentWeek = nextToLastWeekDay.IsInCurrentWeek(startDayOfWeek);
            Assert.False(nextToLastDayIsInCurrentWeek);
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

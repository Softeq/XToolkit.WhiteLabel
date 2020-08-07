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
    }
}

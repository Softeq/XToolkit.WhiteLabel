// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using Softeq.XToolkit.Common.iOS.Extensions;
using Xunit;

namespace Softeq.XToolkit.Common.iOS.Tests.Extensions.DateTimeExtensionsTests
{
    public class DateTimeExtensionsTests
    {
        [Theory]
        [InlineData(-100000)]
        [InlineData(0)]
        [InlineData(100000)]
        public void ToUtcDateTime_ForNsDate_ConvertsToUtcDateTime(double secondsSinceNow)
        {
            var nsDate = NSDate.FromTimeIntervalSinceNow(secondsSinceNow);
            var calendar = NSCalendar.CurrentCalendar;
            calendar.TimeZone = NSTimeZone.FromGMT(0);
            var utcComponents = calendar.Components(
                NSCalendarUnit.Second | NSCalendarUnit.Minute | NSCalendarUnit.Hour |
                NSCalendarUnit.Day | NSCalendarUnit.Month | NSCalendarUnit.Year, nsDate);

            var dateTime = nsDate.ToUtcDateTime();

            Assert.IsType<DateTime>(dateTime);
            Assert.Equal(DateTimeKind.Utc, dateTime.Kind);
            Assert.Equal(utcComponents.Year, dateTime.Year);
            Assert.Equal(utcComponents.Month, dateTime.Month);
            Assert.Equal(utcComponents.Day, dateTime.Day);
            Assert.Equal(utcComponents.Hour, dateTime.Hour);
            Assert.Equal(utcComponents.Minute, dateTime.Minute);
            Assert.Equal(utcComponents.Second, dateTime.Second);
        }

        [Theory]
        [InlineData(-100000)]
        [InlineData(0)]
        [InlineData(100000)]
        public void ToLocalDateTime_ForNsDate_ConvertsToLocalDateTime(double secondsSinceNow)
        {
            var nsDate = NSDate.FromTimeIntervalSinceNow(secondsSinceNow);
            var localComponents = NSCalendar.CurrentCalendar.Components(
                NSCalendarUnit.Second | NSCalendarUnit.Minute | NSCalendarUnit.Hour |
                NSCalendarUnit.Day | NSCalendarUnit.Month | NSCalendarUnit.Year, nsDate);

            var dateTime = nsDate.ToLocalDateTime();

            Assert.IsType<DateTime>(dateTime);
            Assert.Equal(DateTimeKind.Local, dateTime.Kind);
            Assert.Equal(localComponents.Year, dateTime.Year);
            Assert.Equal(localComponents.Month, dateTime.Month);
            Assert.Equal(localComponents.Day, dateTime.Day);
            Assert.Equal(localComponents.Hour, dateTime.Hour);
            Assert.Equal(localComponents.Minute, dateTime.Minute);
            Assert.Equal(localComponents.Second, dateTime.Second);
        }

        //[Theory]
        //[InlineData(-100000)]
        //[InlineData(0)]
        //[InlineData(100000)]
        //public void ToLocalDateTimeAndBack_ForNsDate_ReturnsSameNsDate(double secondsSinceNow)
        //{
        //    var nsDate = NSDate.FromTimeIntervalSinceNow(secondsSinceNow);

        //    var dateTime = nsDate.ToLocalDateTime();
        //    var newNsDate = dateTime.ToNsDate();

        //    Assert.Equal(nsDate, newNsDate);
        //}

        [Theory]
        [InlineData(-100000)]
        [InlineData(0)]
        [InlineData(100000)]
        public void ToDateTimeOffsetLocal_ForNsDate_ConvertsToDateTimeOffset(double secondsSinceNow)
        {
            var nsDate = NSDate.FromTimeIntervalSinceNow(secondsSinceNow);
            var components = NSCalendar.CurrentCalendar.Components(
                NSCalendarUnit.Second | NSCalendarUnit.Minute | NSCalendarUnit.Hour |
                NSCalendarUnit.Day | NSCalendarUnit.Month | NSCalendarUnit.Year, nsDate);

            var dateTimeOffset = nsDate.ToDateTimeOffsetLocal();

            Assert.IsType<DateTimeOffset>(dateTimeOffset);
            Assert.Equal(components.Year, dateTimeOffset.Year);
            Assert.Equal(components.Month, dateTimeOffset.Month);
            Assert.Equal(components.Day, dateTimeOffset.Day);
            Assert.Equal(components.Hour, dateTimeOffset.Hour);
            Assert.Equal(components.Minute, dateTimeOffset.Minute);
            Assert.Equal(components.Second, dateTimeOffset.Second);
        }

        [Theory]
        [InlineData(-100000)]
        [InlineData(0)]
        [InlineData(100000)]
        public void ToDateTimeOffsetLocalAndBack_ForNsDate_ReturnsSameNsDate(double secondsSinceNow)
        {
            var nsDate = NSDate.FromTimeIntervalSinceNow(secondsSinceNow);

            var dateTime = nsDate.ToDateTimeOffsetLocal();
            var newNsDate = dateTime.ToNsDateLocal();

            Assert.Equal(nsDate, newNsDate);
        }
    }
}

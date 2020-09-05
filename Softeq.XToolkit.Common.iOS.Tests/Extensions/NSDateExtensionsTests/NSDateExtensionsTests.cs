// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using Softeq.XToolkit.Common.iOS.Extensions;
using Xunit;

namespace Softeq.XToolkit.Common.iOS.Tests.Extensions.NSDateExtensionsTests
{
    public class NSDateExtensionsTests
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
            var utcComponents = CreateComponents(nsDate, calendar);

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
            var localComponents = CreateComponents(nsDate, NSCalendar.CurrentCalendar);

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

        [Theory]
        [InlineData(-100000)]
        [InlineData(0)]
        [InlineData(100000)]
        public void ToLocalDateTimeAndBack_ForNsDate_ReturnsSameNsDate(double secondsSinceNow)
        {
            var nsDate = NSDate.FromTimeIntervalSinceNow(secondsSinceNow);

            var dateTime = nsDate.ToLocalDateTime();
            var newNsDate = dateTime.ToNsDate();

            Assert.Equal(nsDate.SecondsSince1970, newNsDate.SecondsSince1970, 3);
        }

        private NSDateComponents CreateComponents(NSDate date, NSCalendar calendar)
        {
            return calendar.Components(
                NSCalendarUnit.Second | NSCalendarUnit.Minute | NSCalendarUnit.Hour |
                NSCalendarUnit.Day | NSCalendarUnit.Month | NSCalendarUnit.Year, date);
        }
    }
}

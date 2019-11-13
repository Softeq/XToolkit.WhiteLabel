// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;

namespace Softeq.XToolkit.Common.iOS.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTime(this NSDate date)
        {
            return ((DateTime) date).ToLocalTime();
        }

        public static NSDate ToNsDate(this DateTime date)
        {
            return (NSDate) DateTime.SpecifyKind(date, DateTimeKind.Utc);
        }

        public static NSDate ToNsDateLocal(this DateTimeOffset dateTimeOffset)
        {
            var timeSpan = dateTimeOffset - new DateTimeOffset(2001, 1, 1, 0, 0, 0, DateTimeOffset.UtcNow.Offset);
            var nsDate = NSDate.FromTimeIntervalSinceReferenceDate(timeSpan.TotalSeconds);
            return nsDate;
        }

        public static DateTimeOffset ToDateTimeOffsetLocal(this NSDate date)
        {
            var dateTimeOffset = new DateTimeOffset(2001, 1, 1, 0, 0, 0, DateTimeOffset.Now.Offset)
                .AddSeconds(date.SecondsSinceReferenceDate)
                .AddSeconds(DateTimeOffset.Now.Offset.TotalSeconds);
            return dateTimeOffset;
        }
    }
}

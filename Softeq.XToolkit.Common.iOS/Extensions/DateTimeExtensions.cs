// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;

namespace Softeq.XToolkit.Common.iOS.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime NSDateToDateTime(this NSDate date)
        {
            var reference = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));
            return reference.AddSeconds(date.SecondsSinceReferenceDate);
        }

        public static NSDate DateTimeToNSDate(this DateTime date)
        {
            var reference = new DateTime(2001, 1, 1, 0, 0, 0);
            return NSDate.FromTimeIntervalSinceReferenceDate(
                (date - reference).TotalSeconds);
        }
        
        public static NSDate ToNSDateLocal(this DateTimeOffset dateTimeOffset)
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
// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;

namespace Softeq.XToolkit.WhiteLabel.Tests.Services.NewtonsoftJsonSerializerTests
{
    public static class NewtonsoftJsonSerializerTestsSerializeDataProvider
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                yield return new object[]
                {
                    new StubSerializerData
                    {
                        Name = "Test",
                        Age = 1,
                        Date = new DateTime(2022, 09, 18),
                        Time = new TimeSpan(10, 25, 38)
                    },
                    "{\"name\":\"Test\",\"age\":1,\"date\":\"2022-09-18T00:00:00Z\",\"time\":\"10:25:38\"}"
                };
                yield return new object[]
                {
                    new StubSerializerData
                    {
                        Name = string.Empty
                    },
                    "{\"name\":\"\"}"
                };
                yield return new object[]
                {
                    new StubSerializerData
                    {
                        Age = null,
                        Date = null,
                        Time = null,
                    },
                    "{}"
                };
                yield return new object[]
                {
                    new StubSerializerData { Name = "Test" },
                    "{\"name\":\"Test\"}"
                };
                yield return new object[]
                {
                    new StubSerializerData { FirstName = "First", last_name = "Last" },
                    "{\"first_name\":\"First\",\"last_name\":\"Last\"}"
                };
                yield return new object[]
                {
                    new StubSerializerData { IgnoreData = "IgnoreData" },
                    "{}"
                };
                yield return new object[]
                {
                    new StubSerializerData
                    {
                        Date = TimeZoneInfo.ConvertTimeFromUtc(
                            new DateTime(2022, 10, 16, 10, 34, 47),
                            TimeZoneInfo.FindSystemTimeZoneById("Europe/Minsk")) // UTC+3
                    },
                    "{\"date\":\"2022-10-16T13:34:47Z\"}"
                };
            }
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;

#pragma warning disable SA1122

namespace Softeq.XToolkit.WhiteLabel.Tests.Services.JsonSerializers;

internal static class JsonSerializationDataProvider
{
    public static IEnumerable<object[]> Data
    {
        get
        {
            yield return new object[]
            {
                new JsonSerializationTestsStub
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
                new JsonSerializationTestsStub
                {
                    Name = string.Empty
                },
                "{\"name\":\"\"}"
            };
            yield return new object[]
            {
                new JsonSerializationTestsStub
                {
                    Date = TimeZoneInfo.ConvertTimeFromUtc(
                        new DateTime(2023, 08, 22, 20, 15, 00),
                        TimeZoneInfo.FindSystemTimeZoneById("Europe/Minsk")), // UTC+3
                },
                "{\"date\":\"2023-08-22T23:15:00Z\"}"
            };
            yield return new object[]
            {
                new JsonSerializationTestsStub
                {
                    Age = null,
                    Date = null,
                    Time = null,
                },
                "{}"
            };
            yield return new object[]
            {
                new JsonSerializationTestsStub
                {
                    Name = "Test"
                },
                "{\"name\":\"Test\"}"
            };
            yield return new object[]
            {
                new JsonSerializationTestsStub
                {
                    FirstName = "First",
                    last_name = "Last"
                },
                "{\"first_name\":\"First\",\"last_name\":\"Last\"}"
            };
            yield return new object[]
            {
                new JsonSerializationTestsStub
                {
                    IgnoreData = "IgnoreData"
                },
                "{}"
            };
            yield return new object[]
            {
                new JsonSerializationTestsStub
                {
                    Date = TimeZoneInfo.ConvertTimeFromUtc(
                        new DateTime(2022, 10, 16, 10, 34, 47),
                        TimeZoneInfo.FindSystemTimeZoneById("Europe/Minsk")) // UTC+3
                },
                "{\"date\":\"2022-10-16T13:34:47Z\"}"
            };
            yield return new object[]
            {
                1,
                "1"
            };
            yield return new object[]
            {
                2.5,
                "2.5"
            };
            yield return new object[]
            {
                "123",
                "\"123\""
            };
            yield return new object[]
            {
                "TestString",
                "\"TestString\""
            };
            yield return new object[]
            {
                new Dictionary<string, object>
                {
                    { "first", "TestString" },
                    { "Second", 123 },
                },
                "{\"first\":\"TestString\",\"Second\":123}"
            };
            yield return new object[]
            {
                new DateTime(2022, 09, 18),
                "\"2022-09-18T00:00:00Z\""
            };
            yield return new object[]
            {
                new byte[] { 127, 0, 0, 1 },
                "\"fwAAAQ==\""
            };
            yield return new object[]
            {
                new List<byte> { 127, 0, 0, 1 },
                "[127,0,0,1]"
            };
            yield return new object[]
            {
                new EnumJsonSerializationTestsStub
                {
                    AsString = JsonSerializationTestsEnum.StringValue,
                    AsNumber = JsonSerializationTestsEnum.NumberValue
                },
                "{\"asString\":\"StringValue\",\"asNumber\":123}"
            };
            yield return new object[]
            {
                JsonSerializationTestsEnum.NumberValue,
                123
            };
            yield return new object[]
            {
                JsonSerializationTestsEnum.StringValue,
                1
            };
            yield return new object[]
            {
                null,
                "null"
            };
            yield return new object[]
            {
                "",
                "\"\""
            };
            yield return new object[]
            {
                new { total = "123" },
                "{\"total\":\"123\"}"
            };

            var child = new JsonSerializationTestsStubRef { Id = "child" };
            yield return new object[]
            {
                new JsonSerializationTestsStubRef
                {
                    Id = "root",
                    Child = child,
                    Children = new List<JsonSerializationTestsStubRef> { child }
                },
                "{\"id\":\"root\",\"child\":{\"id\":\"child\"},\"children\":[{\"id\":\"child\"}]}"
            };
        }
    }
}

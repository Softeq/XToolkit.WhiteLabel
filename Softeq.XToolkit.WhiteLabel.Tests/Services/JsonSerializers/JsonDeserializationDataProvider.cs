// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Softeq.XToolkit.WhiteLabel.Tests.Services.JsonSerializers;

#pragma warning disable SA1122

internal static class JsonDeserializationDataProvider
{
    public static IEnumerable<object[]> Data
    {
        get
        {
            yield return new object[]
            {
                "{\"name\":\"Test\",\"age\":1,\"date\":\"2022-09-18T00:00:00Z\",\"time\":\"10:25:38\"}",
                new JsonSerializationTestsStub
                {
                    Name = "Test",
                    Age = 1,
                    Date = new DateTime(2022, 09, 18),
                    Time = new TimeSpan(10, 25, 38)
                }
            };
            yield return new object[]
            {
                "{\"age\":\"2\"}",
                new JsonSerializationTestsStub
                {
                    Age = 2
                }
            };
            yield return new object[]
            {
                "{\"name\":\"\"}",
                new JsonSerializationTestsStub
                {
                    Name = string.Empty
                }
            };
            yield return new object[]
            {
                "{\"first_name\":\"First\",\"last_name\":\"Last\"}",
                new JsonSerializationTestsStub
                {
                    FirstName = "First",
                    last_name = "Last"
                }
            };
            yield return new object[]
            {
                "{}",
                new JObject()
            };
            yield return new object[]
            {
                "",
                null
            };
            yield return new object[]
            {
                "1",
                1
            };
            yield return new object[]
            {
                "2.5",
                2.5
            };
            yield return new object[]
            {
                "\"123\"",
                "123"
            };
            yield return new object[]
            {
                "\"TestString\"",
                "TestString"
            };
            yield return new object[]
            {
                "{\"first\":\"TestString\"}",
                new Dictionary<string, string>
                {
                    { "first", "TestString" }
                }
            };
            yield return new object[]
            {
                "\"2022-09-18T00:00:00Z\"",
                new DateTime(2022, 09, 18)
            };
            yield return new object[]
            {
                "\"fwAAAQ==\"",
                new byte[] { 127, 0, 0, 1 },
            };
            yield return new object[]
            {
                "[127,0,0,1]",
                new List<byte> { 127, 0, 0, 1 }
            };
            yield return new object[]
            {
                "{\"asString\":\"StringValue\",\"asNumber\":123}",
                new EnumJsonSerializationTestsStub
                {
                    AsString = JsonSerializationTestsEnum.StringValue,
                    AsNumber = JsonSerializationTestsEnum.NumberValue
                },
            };
            yield return new object[]
            {
                123,
                JsonSerializationTestsEnum.NumberValue
            };
            yield return new object[]
            {
                1,
                JsonSerializationTestsEnum.StringValue
            };
        }
    }

    public static IEnumerable<object[]> InvalidData
    {
        get
        {
            yield return new object[]
            {
                "2022-10-18T00:00:00Z",
                new DateTime(2022, 10, 18)
            };
            yield return new object[]
            {
                "TestString",
                "ignore"
            };
            yield return new object[]
            {
                "{\"a:123/* comment*/}",
                "ignore"
            };
            yield return new object[]
            {
                "{\"a:123 // comment}",
                "ignore"
            };
        }
    }

    public static IEnumerable<object[]> CustomObjects
    {
        get
        {
            yield return new object[]
            {
                "{\"name\":\"Test\",\"age\":1}",
                new JsonSerializationTestsStub
                {
                    Name = "Test",
                    Age = 1,
                }
            };
            yield return new object[]
            {
                "{\"age\":\"2\"}",
                new JsonSerializationTestsStub
                {
                    Age = 2
                }
            };
            yield return new object[]
            {
                "{\"name\":\"\"}",
                new JsonSerializationTestsStub
                {
                    Name = string.Empty
                }
            };
            yield return new object[]
            {
                "2.5",
                2.5
            };
            yield return new object[]
            {
                "\"123\"",
                "123"
            };
        }
    }
}

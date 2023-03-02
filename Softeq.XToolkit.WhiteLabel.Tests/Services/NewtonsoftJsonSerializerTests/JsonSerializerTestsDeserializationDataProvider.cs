﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Softeq.XToolkit.WhiteLabel.Tests.Services.NewtonsoftJsonSerializerTests;

#pragma warning disable SA1122

public static class JsonSerializerTestsDeserializationDataProvider
{
    public static IEnumerable<object[]> Data
    {
        get
        {
            yield return new object[]
            {
                "{\"name\":\"Test\",\"age\":1,\"date\":\"2022-09-18T00:00:00Z\",\"time\":\"10:25:38\"}",
                new StubSerializerData
                {
                    Name = "Test",
                    Age = 1,
                    Date = new DateTime(2022, 09, 18),
                    Time = new TimeSpan(10, 25, 38)
                }
            };
            yield return new object[]
            {
                "{\"name\":\"\"}",
                new StubSerializerData
                {
                    Name = string.Empty
                }
            };
            yield return new object[]
            {
                "{\"first_name\":\"First\",\"last_name\":\"Last\"}",
                new StubSerializerData
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
                "\"TestString\"",
                "TestString"
            };
            yield return new object[]
            {
                "\"2022-09-18T00:00:00Z\"",
                new DateTime(2022, 09, 18)
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
                "TestString"
            };
        }
    }
}

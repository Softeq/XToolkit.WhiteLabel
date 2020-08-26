// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.GenericEventArgsTests
{
    public class GenericEventArgsTests
    {
        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { null },
            new object[] { 123 },
            new object[] { "test string" }
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void Constructor_WithParameter_ReturnsEventArgs<T>(T parameter)
        {
            var obj = new GenericEventArgs<T>(parameter);

            Assert.IsAssignableFrom<EventArgs>(obj);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Value_WithAnyParameter_ReturnsParameter<T>(T parameter)
        {
            var obj = new GenericEventArgs<T>(parameter);

            var result = obj.Value;

            Assert.Equal(parameter, result);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void From_WithAnyParameter_ReturnsGenericEventArgs<T>(T parameter)
        {
            var result = GenericEventArgs.From(parameter);

            Assert.IsAssignableFrom<GenericEventArgs<T>>(result);
        }
    }
}

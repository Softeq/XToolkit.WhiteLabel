// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Common.Extensions;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Extensions.EnumExtensionsTests
{
    public class EnumExtensionsTests
    {
        private readonly Action<TestEnum> _testAction = Substitute.For<Action<TestEnum>>();

        [Theory]
        [InlineData("Value1", TestEnum.Value1)]
        [InlineData("Value2", TestEnum.Value2)]
        [InlineData("Value3", TestEnum.Value3)]
        [InlineData("Value4", TestEnum.Value4)]
        [InlineData("Value5", TestEnum.Value5)]
        public void Parse_ExistingEnumValue_ParsesCorrectly(string value, Enum expectedResult)
        {
            var result = value.Parse<TestEnum>();

            Assert.IsType<TestEnum>(result);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("value1")]
        [InlineData("VALUE2")]
        [InlineData("Value 4")]
        [InlineData("abracadabra")]
        public void Parse_NonExistingEnumValue_ThrowsArgumentException(string value)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var result = value.Parse<TestEnum>();
            });
        }

        [Fact]
        public void Parse_NullValue_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = EnumExtensions.Parse<TestEnum>(null);
            });
        }

        [Theory]
        [InlineData("Value1", TestEnum.Value1)]
        [InlineData("Value2", TestEnum.Value2)]
        [InlineData("Value3", TestEnum.Value3)]
        [InlineData("Value4", TestEnum.Value4)]
        [InlineData("Value5", TestEnum.Value5)]
        public void ParseWithIgnoreCaseFalse_ExistingEnumValue_ParsesCorrectly(string value, Enum expectedResult)
        {
            var result = value.Parse<TestEnum>(false);

            Assert.IsType<TestEnum>(result);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("value1")]
        [InlineData("VALUE2")]
        [InlineData("Value 4")]
        [InlineData("abracadabra")]
        public void ParseWithIgnoreCaseFalse_NonExistingEnumValue_ThrowsArgumentException(string value)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var result = value.Parse<TestEnum>(false);
            });
        }

        [Fact]
        public void ParseWithIgnoreCaseFalse_NullValue_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = EnumExtensions.Parse<TestEnum>(null, false);
            });
        }

        [Theory]
        [InlineData("Value1", TestEnum.Value1)]
        [InlineData("value2", TestEnum.Value2)]
        [InlineData("VALUE3", TestEnum.Value3)]
        [InlineData("VaLuE4", TestEnum.Value4)]
        [InlineData("Value5", TestEnum.Value5)]
        public void ParseWithIgnoreCaseTrue_ExistingEnumValue_ParsesCorrectly(string value, Enum expectedResult)
        {
            var result = value.Parse<TestEnum>(true);

            Assert.IsType<TestEnum>(result);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Value 4")]
        [InlineData("abracadabra")]
        public void ParseWithIgnoreCaseTrue_NonExistingEnumValue_ThrowsArgumentException(string value)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var result = value.Parse<TestEnum>(true);
            });
        }

        [Fact]
        public void ParseWithIgnoreCaseTrue_NullValue_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = EnumExtensions.Parse<TestEnum>(null, true);
            });
        }

        [Fact]
        public void Apply_NullAction_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                EnumExtensions.Apply<TestEnum>(null);
            });
        }

        [Fact]
        public void Apply_NotNullAction_ActionIsCalledForEveryEnumValue()
        {
            _testAction.ClearReceivedCalls();
            var enumValues = Enum.GetValues(typeof(TestEnum));

            _testAction.Apply<TestEnum>();

            _testAction.ReceivedWithAnyArgs(enumValues.Length).Invoke(Arg.Any<TestEnum>());
        }

        [Theory]
        [InlineData(TestEnum.Value1)]
        [InlineData(TestEnum.Value2)]
        public void GetDescrition_NoDescription_ReturnsNull(Enum value)
        {
            var result = value.GetDescription();

            Assert.Null(result);
        }

        [Theory]
        [InlineData(TestEnum.Value4, "value number 4")]
        [InlineData(TestEnum.Value5, "Description")]
        public void GetDescrition_WithDescription_ReturnsValidString(Enum value, string description)
        {
            var result = value.GetDescription();

            Assert.NotNull(result);
            Assert.Equal(description, result);
        }

        [Theory]
        [InlineData(0, TestEnum.Value1)]
        [InlineData(1, TestEnum.Value2)]
        [InlineData(42, TestEnum.Value3)]
        [InlineData(4, TestEnum.Value4)]
        [InlineData(5, TestEnum.Value5)]
        public void FindByValue_ExistingEnumValue_ReturnsCorrespondingEnum(byte value, Enum expectedResult)
        {
            var result = value.FindByValue<TestEnum>();

            Assert.IsType<TestEnum>(result);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData((byte) 2)]
        [InlineData((byte) 3)]
        [InlineData((byte) 6)]
        [InlineData((byte) 10)]
        public void FindByValue_NonExistingEnumValue_ThrowsArgumentException(byte value)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var result = value.FindByValue<TestEnum>();
            });
        }

        [Theory]
        [InlineData(TestFlagsEnum.Flag1, TestFlagsEnum.Flag2, (TestFlagsEnum) 3)]
        [InlineData(TestFlagsEnum.Flag16, TestFlagsEnum.Flag4, (TestFlagsEnum) 20)]
        [InlineData(TestFlagsEnum.Flag4, TestFlagsEnum.Flag8, (TestFlagsEnum) 12)]
        [InlineData((TestFlagsEnum) 20, TestFlagsEnum.Flag2, (TestFlagsEnum) 22)]
        [InlineData((TestFlagsEnum) 12, (TestFlagsEnum) 3, (TestFlagsEnum) 15)]
        public void SetFlag_ForGivenFlags_ReturnsExpectedResult(TestFlagsEnum data, TestFlagsEnum flags, TestFlagsEnum expectedResult)
        {
            var result = data.SetFlag(flags);

            Assert.Equal(expectedResult, result);
        }
    }
}

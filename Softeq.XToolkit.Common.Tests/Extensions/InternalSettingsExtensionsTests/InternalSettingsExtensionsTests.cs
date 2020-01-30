// Developed by Softeq Development Corporation
// http://www.softeq.com

using NSubstitute;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Interfaces;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Extensions.InternalSettingsExtensionsTests
{
    public class InternalSettingsExtensionsTests
    {
        [Fact]
        public void AddOrUpdateJsonValue_NullValue_RemovesKey()
        {
            var internalSettings = Substitute.For<IInternalSettings>();
            var jsonSerializer = Substitute.For<IJsonSerializer>();
            var key = InternalSettingsExtensionsHelper.Key;
            var value = InternalSettingsExtensionsHelper.NullTestObject;

            internalSettings.AddOrUpdateJsonValue(jsonSerializer, key, value);

            internalSettings.Received().Remove(key);
            jsonSerializer.DidNotReceive().Serialize(Arg.Any<InternalSettingsExtensionsHelper.TestObject>());
            internalSettings.DidNotReceive().AddOrUpdateValue(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void AddOrUpdateJsonValue_NotNullValue_AddsOrUpdatesKey()
        {
            var internalSettings = Substitute.For<IInternalSettings>();
            var jsonSerializer = Substitute.For<IJsonSerializer>();
            var key = InternalSettingsExtensionsHelper.Key;
            var value = InternalSettingsExtensionsHelper.NotNullTestObject;
            var json = "json";
            jsonSerializer.Serialize(value).Returns(json);

            internalSettings.AddOrUpdateJsonValue(jsonSerializer, key, value);

            jsonSerializer.Received().Serialize(value);
            internalSettings.Received().AddOrUpdateValue(key, json);
            internalSettings.DidNotReceive().Remove(Arg.Any<string>());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetJsonValueOrDefault_EmptyJsonAndDefaultNotSpecified_ReturnsDefaultValue(string json)
        {
            var internalSettings = Substitute.For<IInternalSettings>();
            var jsonSerializer = Substitute.For<IJsonSerializer>();
            var key = InternalSettingsExtensionsHelper.Key;
            var expectedValue = InternalSettingsExtensionsHelper.NullTestObject;
            internalSettings.GetValueOrDefault(key, Arg.Any<string>()).Returns(json);

            var value = internalSettings.GetJsonValueOrDefault<InternalSettingsExtensionsHelper.TestObject>(jsonSerializer, key);

            internalSettings.Received().GetValueOrDefault(key, default(string));
            jsonSerializer.DidNotReceive().Deserialize<InternalSettingsExtensionsHelper.TestObject>(Arg.Any<string>());
            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetJsonValueOrDefault_EmptyJsonAndDefaultSpecified_ReturnsSpecifiedDefaultValue(string json)
        {
            var internalSettings = Substitute.For<IInternalSettings>();
            var jsonSerializer = Substitute.For<IJsonSerializer>();
            var key = InternalSettingsExtensionsHelper.Key;
            var expectedValue = InternalSettingsExtensionsHelper.NotNullTestObject;
            internalSettings.GetValueOrDefault(key, Arg.Any<string>()).Returns(json);

            var value = internalSettings.GetJsonValueOrDefault(jsonSerializer, key, expectedValue);

            internalSettings.Received().GetValueOrDefault(key, default(string));
            jsonSerializer.DidNotReceive().Deserialize<InternalSettingsExtensionsHelper.TestObject>(Arg.Any<string>());
            Assert.Equal(expectedValue, value);
        }

        [Fact]
        public void GetJsonValueOrDefault_NotEmptyJson_ReturnsSerializedValue()
        {
            var internalSettings = Substitute.For<IInternalSettings>();
            var jsonSerializer = Substitute.For<IJsonSerializer>();
            var key = InternalSettingsExtensionsHelper.Key;
            var expectedValue = InternalSettingsExtensionsHelper.NotNullTestObject;
            var json = "json";
            internalSettings.GetValueOrDefault(key, Arg.Any<string>()).Returns(json);
            jsonSerializer.Deserialize<InternalSettingsExtensionsHelper.TestObject>(json).Returns(expectedValue);

            var value = internalSettings.GetJsonValueOrDefault<InternalSettingsExtensionsHelper.TestObject>(jsonSerializer, key);

            internalSettings.Received().GetValueOrDefault(key, default(string));
            jsonSerializer.Received().Deserialize<InternalSettingsExtensionsHelper.TestObject>(json);
            Assert.Equal(expectedValue, value);
        }
    }
}

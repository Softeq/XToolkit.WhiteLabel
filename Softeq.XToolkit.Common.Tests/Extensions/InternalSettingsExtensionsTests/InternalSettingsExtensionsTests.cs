// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Interfaces;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Extensions.InternalSettingsExtensionsTests
{
    public class InternalSettingsExtensionsTests : IDisposable
    {
        private readonly IInternalSettings _internalSettings;
        private readonly IJsonSerializer _jsonSerializer;

        private readonly string _key;
        private readonly TestObject _nullValue;
        private readonly TestObject _notNullValue;
        private readonly string _notEmptyJson;

        public InternalSettingsExtensionsTests()
        {
            _internalSettings = Substitute.For<IInternalSettings>();
            _jsonSerializer = Substitute.For<IJsonSerializer>();

            _key = "test_key";
            _nullValue = null;
            _notNullValue = new TestObject();
            _notEmptyJson = "json";
        }

        public void Dispose()
        {
        }

        [Fact]
        public void AddOrUpdateJsonValue_NullValue_RemovesKey()
        {
            _internalSettings.AddOrUpdateJsonValue(_jsonSerializer, _key, _nullValue);

            _internalSettings.Received().Remove(_key);
            _jsonSerializer.DidNotReceive().Serialize(Arg.Any<TestObject>());
            _internalSettings.DidNotReceive().AddOrUpdateValue(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void AddOrUpdateJsonValue_NotNullValue_AddsOrUpdatesKey()
        {
            _jsonSerializer.Serialize(_notNullValue).Returns(_notEmptyJson);

            _internalSettings.AddOrUpdateJsonValue(_jsonSerializer, _key, _notNullValue);

            _jsonSerializer.Received().Serialize(_notNullValue);
            _internalSettings.Received().AddOrUpdateValue(_key, _notEmptyJson);
            _internalSettings.DidNotReceive().Remove(Arg.Any<string>());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetJsonValueOrDefault_EmptyJsonAndDefaultNotSpecified_ReturnsDefaultValue(string json)
        {
            _internalSettings.GetValueOrDefault(_key, Arg.Any<string>()).Returns(json);

            var actualValue = _internalSettings.GetJsonValueOrDefault<TestObject>(_jsonSerializer, _key);

            _internalSettings.Received().GetValueOrDefault(_key, default(string));
            _jsonSerializer.DidNotReceive().Deserialize<TestObject>(Arg.Any<string>());
            Assert.Equal(_nullValue, actualValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetJsonValueOrDefault_EmptyJsonAndDefaultSpecified_ReturnsSpecifiedDefaultValue(string json)
        {
            _internalSettings.GetValueOrDefault(_key, Arg.Any<string>()).Returns(json);

            var actualValue = _internalSettings.GetJsonValueOrDefault(_jsonSerializer, _key, _nullValue);

            _internalSettings.Received().GetValueOrDefault(_key, default(string));
            _jsonSerializer.DidNotReceive().Deserialize<TestObject>(Arg.Any<string>());
            Assert.Equal(_nullValue, actualValue);
        }

        [Fact]
        public void GetJsonValueOrDefault_NotEmptyJson_ReturnsSerializedValue()
        {
            _internalSettings.GetValueOrDefault(_key, Arg.Any<string>()).Returns(_notEmptyJson);
            _jsonSerializer.Deserialize<TestObject>(_notEmptyJson).Returns(_nullValue);

            var actualValue = _internalSettings.GetJsonValueOrDefault<TestObject>(_jsonSerializer, _key);

            _internalSettings.Received().GetValueOrDefault(_key, default(string));
            _jsonSerializer.Received().Deserialize<TestObject>(_notEmptyJson);
            Assert.Equal(_nullValue, actualValue);
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using Newtonsoft.Json;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.WhiteLabel.Services;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Services.NewtonsoftJsonSerializerTests
{
    public class NewtonsoftJsonSerializerTests
    {
        private readonly NewtonsoftJsonSerializer _serializer;

        public NewtonsoftJsonSerializerTests()
        {
            _serializer = new NewtonsoftJsonSerializer();
        }

        [Fact]
        public void DefaultSettings_StaticGet_ReturnsJsonSerializerSettings()
        {
            Assert.IsType<JsonSerializerSettings>(NewtonsoftJsonSerializer.DefaultSettings);
        }

        [Fact]
        public void Ctor_Default_ReturnsIJsonSerializer()
        {
            Assert.IsAssignableFrom<IJsonSerializer>(new NewtonsoftJsonSerializer());
        }

        [Theory]
        [MemberData(
            nameof(NewtonsoftJsonSerializerTestsSerializeDataProvider.Data),
            MemberType = typeof(NewtonsoftJsonSerializerTestsSerializeDataProvider))]
        public void Serialize_CorrectData_ExpectedResult(StubSerializerData data, string expected)
        {
            var result = _serializer.Serialize(data);

            Assert.Equal(expected, result);
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.WhiteLabel.Services;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Services.NewtonsoftJsonSerializerTests;

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
        nameof(JsonSerializerTestsSerializationDataProvider.Data),
        MemberType = typeof(JsonSerializerTestsSerializationDataProvider))]
    public void Serialize_WithValidData_ExpectedResult(object data, string expected)
    {
        var result = _serializer.Serialize(data);

        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(
        nameof(JsonSerializerTestsDeserializationDataProvider.Data),
        MemberType = typeof(JsonSerializerTestsDeserializationDataProvider))]
    public void Deserialize_WithValidData_ExpectedResult<T>(string data, T expected)
    {
        var result = _serializer.Deserialize<T>(data);

        Assert.Equivalent(expected, result);
    }

    [Theory]
    [MemberData(
        nameof(JsonSerializerTestsDeserializationDataProvider.InvalidData),
        MemberType = typeof(JsonSerializerTestsDeserializationDataProvider))]
#pragma warning disable xUnit1026
    public void Deserialize_WithInvalidData_ThrowsException<T>(string data, T expected)
#pragma warning restore xUnit1026
    {
        Assert.Throws<JsonReaderException>(() =>
        {
            _serializer.Deserialize<T>(data);
        });
    }

    [Theory]
    [MemberData(
        nameof(JsonSerializerTestsSerializationDataProvider.Data),
        MemberType = typeof(JsonSerializerTestsSerializationDataProvider))]
    public async Task SerializeAsync_WithValidData_ExpectedResult(object data, string expected)
    {
        using var stream = new MemoryStream();

        await _serializer.SerializeAsync(data, stream);

        var result = Encoding.UTF8.GetString(stream.ToArray());

        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(
        nameof(JsonSerializerTestsDeserializationDataProvider.Data),
        MemberType = typeof(JsonSerializerTestsDeserializationDataProvider))]
    public async Task DeserializeAsync_WithValidData_ExpectedResult<T>(string data, T expected)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));

        var result = await _serializer.DeserializeAsync<T>(stream)!;

        Assert.Equivalent(expected, result);
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.WhiteLabel.Services;
using Softeq.XToolkit.WhiteLabel.Tests.Services.JsonSerializers;
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
    public void Ctor_Default_ReturnsIJsonSerializer()
    {
        Assert.IsAssignableFrom<IJsonSerializer>(_serializer);
    }

    [Fact]
    public void DefaultSettings_StaticGet_ReturnsJsonSerializerSettings()
    {
        Assert.IsType<JsonSerializerSettings>(NewtonsoftJsonSerializer.DefaultSettings);
    }

    [Theory]
    [MemberData(nameof(JsonSerializationDataProvider.Data), MemberType = typeof(JsonSerializationDataProvider))]
    public void Serialize_WithValidData_ExpectedResult(object data, string expected)
    {
        var result = _serializer.Serialize(data);

        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(JsonDeserializationDataProvider.Data), MemberType = typeof(JsonDeserializationDataProvider))]
    public void Deserialize_WithValidData_ExpectedResult<T>(string data, T expected)
    {
        var result = _serializer.Deserialize<T>(data);

        Assert.Equivalent(expected, result);
    }

    [Theory]
    [MemberData(nameof(JsonDeserializationDataProvider.InvalidData), MemberType = typeof(JsonDeserializationDataProvider))]
#pragma warning disable xUnit1026
    public void Deserialize_WithInvalidData_ThrowsException<T>(string data, T expected)
#pragma warning restore xUnit1026
    {
        Assert.Throws<JsonReaderException>(() =>
        {
            _serializer.Deserialize<T>(data);
        });
    }

    [Fact]
    public void Deserialize_Null_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _serializer.Deserialize<object>(null!);
        });
    }

    [Theory]
    [MemberData(nameof(JsonSerializationDataProvider.Data), MemberType = typeof(JsonSerializationDataProvider))]
    public async Task SerializeAsync_WithValidData_ExpectedResult(object data, string expected)
    {
        using var stream = new MemoryStream();

        await _serializer.SerializeAsync(data, stream);

        var result = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(JsonDeserializationDataProvider.Data), MemberType = typeof(JsonDeserializationDataProvider))]
    public async Task DeserializeAsync_WithValidData_ExpectedResult<T>(string data, T expected)
    {
        var bytes = Encoding.UTF8.GetBytes(data);
        using var stream = new MemoryStream(bytes);

        var result = await _serializer.DeserializeAsync<T>(stream)!;

        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task DeserializeAsync_Null_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _serializer.DeserializeAsync<object>(null!));
    }
}

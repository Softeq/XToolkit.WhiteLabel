// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Text.Json;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Tests.Services.JsonSerializers;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Services.DefaultJsonSerializerTests;

public partial class DefaultJsonSerializerTests
{
    [Fact]
    public void Ctor_Default_ReturnsINavigationSerializer()
    {
        Assert.IsAssignableFrom<INavigationSerializer>(_serializer);
    }

    [Theory]
    [MemberData(nameof(JsonDeserializationDataProvider.CustomObjects), MemberType = typeof(JsonDeserializationDataProvider))]
    public void Deserialize_WithCustomObject_ResultsExpectedType<T>(string json, T expected)
    {
        var jsonElement = JsonDocument.Parse(json).RootElement;

        var result = _serializer.Deserialize(jsonElement, typeof(T));

        Assert.Equivalent(expected, result);
    }

    [Fact]
    public void Deserialize_WithNullCustomObject_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _serializer.Deserialize(null, typeof(object));
        });
    }

    [Fact]
    public void Deserialize_WithUnsupportedCustomObject_ThrowsArgumentNullException()
    {
        Assert.Throws<NotSupportedException>(() =>
        {
            _serializer.Deserialize("test string", typeof(string));
        });
    }
}

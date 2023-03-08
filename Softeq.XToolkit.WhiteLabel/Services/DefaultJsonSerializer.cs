// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Interfaces;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Softeq.XToolkit.WhiteLabel.Services;

/// <summary>
///     A <see langword="class"/> implementing <see cref="IJsonSerializer"/> using the System.Text.Json APIs.
/// </summary>
public class DefaultJsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerOptions? _options;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DefaultJsonSerializer"/> class with the specified parameters.
    /// </summary>
    /// <param name="options">The serialization settings to use for the current instance.</param>
    public DefaultJsonSerializer(JsonSerializerOptions? options = null)
    {
        _options = options ?? DefaultOptions;
    }

    /// <summary>
    ///     Gets default serialization options.
    /// </summary>
    public static JsonSerializerOptions DefaultOptions { get; } = CreateDefaultOptions();

    private static JsonSerializerOptions CreateDefaultOptions() => new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = false,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Converters =
        {
            new DateTimeUtcJsonConverter()
        }
    };

    /// <inheritdoc />
    public string Serialize(object value)
    {
        return JsonSerializer.Serialize(value, _options);
    }

    /// <inheritdoc />
    public TResult? Deserialize<TResult>(string value)
    {
        return JsonSerializer.Deserialize<TResult>(value, _options);
    }

    /// <inheritdoc />
    public Task SerializeAsync(object value, Stream stream)
    {
        return JsonSerializer.SerializeAsync(stream, value, _options);
    }

    /// <inheritdoc />
    public Task<TResult?> DeserializeAsync<TResult>(Stream stream)
    {
        return JsonSerializer.DeserializeAsync<TResult>(stream, _options).AsTask();
    }
}

/// <summary>
///     Custom System.Json converter that implements Newtonsoft.Json DateTime settings:
///     (DateFormatHandling.IsoDateFormat, DateTimeZoneHandling.Utc).
/// </summary>
public class DateTimeUtcJsonConverter : JsonConverter<DateTime>
{
    /// <inheritdoc />
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.Parse(reader.GetString() ?? string.Empty).ToUniversalTime();
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
    }
}

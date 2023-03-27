// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.WhiteLabel.Navigation;

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
        Converters =
        {
            new DateTimeUtcJsonConverter()
        },
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = false
    };

    /// <inheritdoc />
    public string Serialize(object value)
    {
        return JsonSerializer.Serialize(value, _options);
    }

    /// <inheritdoc />
    public TResult? Deserialize<TResult>(string? value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (value.Length == 0)
        {
            return default;
        }

        return JsonSerializer.Deserialize<TResult>(value, _options);
    }

    /// <inheritdoc />
    public Task SerializeAsync(object value, Stream stream)
    {
        return JsonSerializer.SerializeAsync(stream, value, _options);
    }

    /// <inheritdoc />
    public async Task<TResult?> DeserializeAsync<TResult>(Stream stream)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        if (stream.Length == 0)
        {
            return default;
        }

        return await JsonSerializer.DeserializeAsync<TResult>(stream, _options).AsTask().ConfigureAwait(false);
    }
}

/// <summary>
///     Custom System.Json converter that implements Newtonsoft.Json DateTime settings:
///     (DateFormatHandling.IsoDateFormat, DateTimeZoneHandling.Utc).
/// </summary>
public sealed class DateTimeUtcJsonConverter : JsonConverter<DateTime>
{
    // Source: https://github.com/JamesNK/Newtonsoft.Json/blob/0a2e291c0d9c0c7675d445703e51750363a549ef/Src/Newtonsoft.Json/Utilities/DateTimeUtils.cs#L85-L86
    private const string IsoDateFormat = "yyyy-MM-ddTHH:mm:ss.FFFFFFFZ";

    /// <inheritdoc />
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateTime = reader.GetDateTime();
        return dateTime.ToUniversalTime();
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var dateTimeString = value.ToString(IsoDateFormat, CultureInfo.InvariantCulture);
        writer.WriteStringValue(dateTimeString);
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.Services
{
    /// <summary>
    /// A <see langword="class"/> implementing <see cref="IJsonSerializer"/> using the Newtonsoft.Json APIs.
    /// </summary>
    public class NewtonsoftJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings _settings;

        /// <summary>
        ///     Initializes a new instance of the <see cref="NewtonsoftJsonSerializer"/> class with the specified parameters.
        /// </summary>
        /// <param name="settings">The serialization settings to use for the current instance.</param>
        public NewtonsoftJsonSerializer(JsonSerializerSettings? settings = null)
        {
            _settings = settings ?? DefaultSettings;
        }

        /// <summary>
        ///     Gets default serialization settings.
        /// </summary>
        public static JsonSerializerSettings DefaultSettings { get; } = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            TypeNameHandling = TypeNameHandling.None,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        /// <inheritdoc />
        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, _settings);
        }

        /// <inheritdoc />
        [return:MaybeNull]
        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, _settings);
        }

        /// <inheritdoc />
        public Task SerializeAsync(object value, Stream stream)
        {
            return Task.Run(() =>
            {
                using (var streamWriter = new StreamWriter(stream))
                using (var jsonTextWriter = new JsonTextWriter(streamWriter))
                {
                    var serializer = JsonSerializer.Create(_settings);
                    serializer.Serialize(jsonTextWriter, value);
                }
            });
        }

        /// <inheritdoc />
        [return:MaybeNull]
        public Task<T> DeserializeAsync<T>(Stream stream)
        {
            return Task.Run(() =>
            {
                using (var streamReader = new StreamReader(stream))
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    var serializer = JsonSerializer.Create(_settings);
                    return serializer.Deserialize<T>(jsonTextReader)!;
                }
            });
        }
    }
}

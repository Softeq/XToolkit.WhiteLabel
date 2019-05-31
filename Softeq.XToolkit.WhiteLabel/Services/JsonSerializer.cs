// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.Services
{
    public class JsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            TypeNameHandling = TypeNameHandling.None,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new DefaultContractResolver {NamingStrategy = new CamelCaseNamingStrategy()},
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, _jsonSerializerSettings);
        }

        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, _jsonSerializerSettings);
        }
        
        public Task<T> DeserializeAsync<T>(Stream stream)
        {
            return Task.Run(() => 
            {
                var serializer = Newtonsoft.Json.JsonSerializer.Create(_jsonSerializerSettings);
                using (var streamReader = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        return serializer.Deserialize<T>(jsonTextReader);
                    }
                }
            });
        }

        public Task SerializeAsync(object obj, Stream stream)
        {
            return Task.Run(() =>
            {
                var serializer = Newtonsoft.Json.JsonSerializer.Create(_jsonSerializerSettings);
                using (var streamWriter = new StreamWriter(stream))
                {
                    using (var jsonTextWriter = new JsonTextWriter(streamWriter))
                    {
                        serializer.Serialize(jsonTextWriter, obj);
                    }
                }
            });
        }
    }
}
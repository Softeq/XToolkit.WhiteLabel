// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.Common.Extensions
{
    public static class InternalSettingsExtensions
    {
        public static void AddOrUpdateJsonValue<T>(this IInternalSettings internalSettings,
            IJsonSerializer jsonSerializer, string key, T value)
        {
            var json = jsonSerializer.Serialize(value);
            if (json == "null")
            {
                internalSettings.Remove(key);
                return;
            }

            internalSettings.AddOrUpdateValue(key, json);
        }

        public static T GetJsonValueOrDefault<T>(this IInternalSettings internalSettings,
            IJsonSerializer jsonSerializer, string key, T defaultValue = default(T))
        {
            var json = internalSettings.GetValueOrDefault(key, default(string));
            return string.IsNullOrEmpty(json)
                ? defaultValue
                : jsonSerializer.Deserialize<T>(json);
        }
    }
}
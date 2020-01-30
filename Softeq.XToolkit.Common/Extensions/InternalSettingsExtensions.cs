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
            if (value == null)
            {
                internalSettings.Remove(key);
                return;
            }

            var json = jsonSerializer.Serialize(value);
            internalSettings.AddOrUpdateValue(key, json);
        }

        public static T GetJsonValueOrDefault<T>(this IInternalSettings internalSettings,
            IJsonSerializer jsonSerializer, string key, T defaultValue = default)
        {
            var json = internalSettings.GetValueOrDefault(key, default(string));
            return string.IsNullOrEmpty(json)
                ? defaultValue
                : jsonSerializer.Deserialize<T>(json);
        }
    }
}

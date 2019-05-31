// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.ComponentModel;

namespace Softeq.XToolkit.Common.Extensions
{
    public static class EnumExtensions
    {
        public static T SetFlag<T>(this Enum value, T flags)
        {
            if (value.GetType() != typeof(T))
            {
                throw new ArgumentException("Enum value and flags types don't match.");
            }

            return (T) Enum.ToObject(typeof(T), Convert.ToUInt64(value) | Convert.ToUInt64(flags));
        }

        public static TEnum Parse<TEnum>(string value)
        {
            return (TEnum) Enum.Parse(typeof(TEnum), value);
        }

        public static TEnum Parse<TEnum>(string value, bool ignoreCase)
        {
            return (TEnum) Enum.Parse(typeof(TEnum), value, ignoreCase);
        }

        public static void Apply<TEnum>(Action<TEnum> action)
        {
            foreach (TEnum constant in Enum.GetValues(typeof(TEnum)))
            {
                action?.Invoke(constant);
            }
        }

        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                var field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
    }
}
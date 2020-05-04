﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.ComponentModel;

namespace Softeq.XToolkit.Common.Extensions
{
    public static class EnumExtensions
    {
        public static TEnum SetFlag<TEnum>(this Enum value, TEnum flags) where TEnum : Enum
        {
            if (value.GetType() != typeof(TEnum))
            {
                throw new ArgumentException("Enum value and flags types don't match.");
            }

            return (TEnum) Enum.ToObject(typeof(TEnum), Convert.ToUInt64(value) | Convert.ToUInt64(flags));
        }

        /// <summary>
        ///     Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent
        ///     enumerated object.
        /// </summary>
        /// <returns>An object of type enumType whose value is represented by value.</returns>
        /// <param name="value">A string containing the name or value to convert..</param>
        /// <typeparam name="TEnum">Enum type.</typeparam>
        public static TEnum Parse<TEnum>(string value) where TEnum : Enum
        {
            return (TEnum) Enum.Parse(typeof(TEnum), value);
        }

        /// <summary>
        ///     Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent
        ///     enumerated object.
        /// </summary>
        /// <returns>An object of type enumType whose value is represented by value.</returns>
        /// <param name="value">A string containing the name or value to convert..</param>
        /// <param name="ignoreCase">true to ignore case; false to regard case.</param>
        /// <typeparam name="TEnum">Enum type.</typeparam>
        public static TEnum Parse<TEnum>(string value, bool ignoreCase) where TEnum : Enum
        {
            return (TEnum) Enum.Parse(typeof(TEnum), value, ignoreCase);
        }

        /// <summary>
        ///     Apply specified action to each value of enum.
        /// </summary>
        /// <param name="action">Specified action.</param>
        /// <typeparam name="TEnum">Enum type.</typeparam>
        public static void Apply<TEnum>(Action<TEnum> action) where TEnum : Enum
        {
            foreach (TEnum constant in Enum.GetValues(typeof(TEnum)))
            {
                action?.Invoke(constant);
            }
        }

        /// <summary>
        ///     Returns <see cref="DescriptionAttribute" /> value of Enum.
        /// </summary>
        /// <param name="value">Enum.</param>
        /// <returns>String of description.</returns>
        public static string? GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }

            var field = type.GetField(name);
            if (field == null)
            {
                return null;
            }

            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
            {
                return attr.Description;
            }

            return null;
        }

        /// <summary>
        ///     Get enumeration that corresponding value.
        /// </summary>
        /// <typeparam name="TEnum">Enum type.</typeparam>
        /// <param name="value">Enum value.</param>
        /// <returns>Correspond enumeration.</returns>
        public static TEnum FindByValue<TEnum>(byte value) where TEnum : Enum
        {
            return (TEnum) Enum.ToObject(typeof(TEnum), value);
        }
    }
}

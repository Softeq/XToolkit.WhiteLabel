// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.ComponentModel;

namespace Softeq.XToolkit.Common.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        ///     Adds a flag to enum value using bitwise OR operation.
        /// </summary>
        /// <typeparam name="TEnum">Enum type.</typeparam>
        /// <param name="value">Initial enum value to add flags to.</param>
        /// <param name="flags">Enum flags to be added.</param>
        /// <returns>Enum value storing both initial value and flags.</returns>
        public static TEnum SetFlag<TEnum>(this TEnum value, TEnum flags) where TEnum : Enum
        {
            return (TEnum) Enum.ToObject(typeof(TEnum), Convert.ToUInt64(value) | Convert.ToUInt64(flags));
        }

        /// <summary>
        ///     Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent
        ///     enumerated object.
        /// </summary>
        /// <returns>An object of type enumType whose value is represented by value.</returns>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <typeparam name="TEnum">Enum type.</typeparam>
        public static TEnum Parse<TEnum>(this string value) where TEnum : Enum
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
        public static TEnum Parse<TEnum>(this string value, bool ignoreCase) where TEnum : Enum
        {
            return (TEnum) Enum.Parse(typeof(TEnum), value, ignoreCase);
        }

        /// <summary>
        ///     Apply specified action to each value of enum.
        /// </summary>
        /// <param name="action">Specified action.</param>
        /// <typeparam name="TEnum">Enum type.</typeparam>
        public static void Apply<TEnum>(this Action<TEnum> action) where TEnum : Enum
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (TEnum constant in Enum.GetValues(typeof(TEnum)))
            {
                action(constant);
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
        ///     Get enumeration that corresponds to byte value.
        /// </summary>
        /// <typeparam name="TEnum">Enum type.</typeparam>
        /// <param name="value">Enum value.</param>
        /// <returns>Corresponding enumeration.</returns>
        public static TEnum FindByValue<TEnum>(this byte value) where TEnum : Enum
        {
            var foundObject = Enum.ToObject(typeof(TEnum), value);

            if (!Enum.IsDefined(typeof(TEnum), foundObject))
            {
                throw new ArgumentException($"{value} value is not defined in {nameof(TEnum)}");
            }

            return (TEnum) foundObject;
        }
    }
}

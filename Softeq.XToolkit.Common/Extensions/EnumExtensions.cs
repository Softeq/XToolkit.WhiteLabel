// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.ComponentModel;

namespace Softeq.XToolkit.Common.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:System.Enum"/>.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        ///     Adds a flag to the current <see cref="T:System.Enum"/> using bitwise OR operation.
        /// </summary>
        /// <typeparam name="TEnum">Enum type.</typeparam>
        /// <param name="value">Initial <see cref="T:System.Enum"/> value to add flags to.</param>
        /// <param name="flags">Flags to be added.</param>
        /// <returns>
        ///     New <see cref="T:System.Enum"/> that stores both initial <see cref="T:System.Enum"/> value and flags.
        ///     Initial <see cref="T:System.Enum"/> is not modified.
        /// </returns>
        public static TEnum SetFlag<TEnum>(this TEnum value, TEnum flags) where TEnum : Enum
        {
            return (TEnum) Enum.ToObject(typeof(TEnum), Convert.ToUInt64(value) | Convert.ToUInt64(flags));
        }

        /// <summary>
        ///     Converts string to the specified <see cref="T:System.Enum"/>.
        /// </summary>
        /// <returns>
        ///     An object of type <typeparamref name="TEnum"/> whose value is represented by <paramref name="value" />.
        /// </returns>
        /// <param name="value">A string to convert.</param>
        /// <typeparam name="TEnum">Enum type.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="value"/> parameters cannot be <see langword="null"/>.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <typeparam name="TEnum" /> is not an <see cref="T:System.Enum" />.
        ///     -or-
        ///     <paramref name="value" /> is either an empty string or only contains white space.
        ///     -or-
        ///     <paramref name="value" /> is a name, but not one of the named constants defined for the enumeration.
        /// </exception>
        /// <exception cref="T:System.OverflowException">
        ///   <paramref name="value" /> is outside the range of the underlying type of <typeparam name="TEnum" />.
        /// </exception>
        public static TEnum Parse<TEnum>(this string value) where TEnum : Enum
        {
            return (TEnum) Enum.Parse(typeof(TEnum), value);
        }

        /// <summary>
        ///     Converts string to the specified <see cref="T:System.Enum"/>.
        /// </summary>
        /// <returns>
        ///     An object of type <typeparamref name="TEnum"/> whose value is represented by <paramref name="value" />.
        /// </returns>
        /// <param name="value">A string to convert.</param>
        /// <param name="ignoreCase"><see langword="true"/> to ignore case; <see langword="false"/> to regard case.</param>
        /// <typeparam name="TEnum">Enum type.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="value" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <typeparam name="TEnum" /> is not an <see cref="T:System.Enum" />.
        ///     -or-
        ///     <paramref name="value" /> is either an empty string ("") or only contains white space.
        ///     -or-
        ///     <paramref name="value" /> is a name, but not one of the named constants defined for the enumeration.
        /// </exception>
        /// <exception cref="T:System.OverflowException">
        ///     <paramref name="value" /> is outside the range of the underlying type of <typeparam name="TEnum" />.
        /// </exception>
        public static TEnum Parse<TEnum>(this string value, bool ignoreCase) where TEnum : Enum
        {
            return (TEnum) Enum.Parse(typeof(TEnum), value, ignoreCase);
        }

        /// <summary>
        ///     Apply the specified action to each value of <see cref="T:System.Enum"/>.
        /// </summary>
        /// <param name="action">Action to apply.</param>
        /// <typeparam name="TEnum"><see cref="T:System.Enum"/> type.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="action"/> parameter cannot be <see langword="null"/>.
        /// </exception>
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
        ///     Returns <see cref="T:System.ComponentModel.DescriptionAttribute"/> value of the specified <see cref="T:System.Enum"/>.
        /// </summary>
        /// <param name="value">Initial <see cref="T:System.Enum"/>.</param>
        /// <returns>Description string.</returns>
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
        ///     Get <see cref="T:System.Enum"/> that corresponds to byte value.
        /// </summary>
        /// <typeparam name="TEnum"><see cref="T:System.Enum"/> type.</typeparam>
        /// <param name="value">Enum value.</param>
        /// <returns>Corresponding <see cref="T:System.Enum"/>.</returns>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="value"/> is not defined in the specified <see cref="T:System.Enum"/>.
        /// </exception>
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

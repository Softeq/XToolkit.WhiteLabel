// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Softeq.XToolkit.Common.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        ///     Try to set the value to property case insensitive.
        /// </summary>
        /// <param name="obj">Owner of property.</param>
        /// <param name="value">Value for set.</param>
        /// <param name="propertyName">Property name.</param>
        public static void SetPropertyValue(object obj, object value, string propertyName)
        {
            var viewModelType = obj.GetType();

            var property = viewModelType.GetPropertyCaseInsensitive(propertyName);

            property?.SetValue(obj, property.PropertyType.CoerceValue(value));
        }

        /// <summary>
        ///     Get property case insensitive from <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Info about the property with the specified name in the specified type.</returns>
        public static PropertyInfo GetPropertyCaseInsensitive(this Type type, string propertyName)
        {
            var typeInfo = type.GetTypeInfo();
            var typeList = new List<Type> { type };

            if (typeInfo.IsInterface)
            {
                typeList.AddRange(typeInfo.ImplementedInterfaces);
            }

            return typeList
                .Select(interfaceType => interfaceType.GetRuntimeProperty(propertyName))
                .FirstOrDefault(property => property != null);
        }

        /// <summary>
        ///     Get coerce value for the type.
        /// </summary>
        /// <param name="type">Destination type.</param>
        /// <param name="value">Provided value.</param>
        /// <returns>Value force converted to the specified type.</returns>
        public static object? CoerceValue(this Type type, object value)
        {
            if (value == null)
            {
                return GetDefaultValue(type);
            }

            var providedType = value.GetType();
            if (type.IsAssignableFrom(providedType))
            {
                return value;
            }

            try
            {
                if (type.IsEnum)
                {
                    if (value is string stringValue)
                    {
                        return Enum.Parse(type, stringValue, true);
                    }

                    return Enum.ToObject(type, value);
                }

                if (typeof(Guid).IsAssignableFrom(type))
                {
                    if (value is string stringValue)
                    {
                        return new Guid(stringValue);
                    }
                }

                return Convert.ChangeType(value, type, CultureInfo.CurrentCulture);
            }
            catch
            {
                return GetDefaultValue(type);
            }
        }

        /// <summary>
        ///     Get default value for <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>Default value.</returns>
        public static object? GetDefaultValue(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }
    }
}

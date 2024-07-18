// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Reflection;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    /// <summary>
    ///     Simple wrapper for <see cref="T:System.Reflection.PropertyInfo"/>
    ///     that contains only property and type names.
    ///     Used for serialization.
    /// </summary>
    public class NavigationPropertyInfo
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NavigationPropertyInfo"/> class.
        /// </summary>
        /// <param name="propertyName">The string containing the name of the public property.</param>
        /// <param name="assemblyQualifiedTypeName">
        ///     The string containing the assembly-qualified name of the type.
        ///     See <see cref="P:System.Type.AssemblyQualifiedName"/>.
        ///     <para/>
        ///     If the type is in the currently executing assembly or in mscorlib.dll/System.Private.CoreLib.dll,
        ///     it is sufficient to supply the type name qualified by its namespace.
        /// </param>
        public NavigationPropertyInfo(string? propertyName, string? assemblyQualifiedTypeName)
        {
            PropertyName = propertyName ?? string.Empty;
            AssemblyQualifiedTypeName = assemblyQualifiedTypeName ?? string.Empty;
        }

        /// <summary>
        ///     Gets the name of the public property.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        ///     Gets the assembly-qualified name of the type.
        ///     See <see cref="P:System.Type.AssemblyQualifiedName"/>.
        ///     <para/>
        ///     If the type is in the currently executing assembly or in mscorlib.dll/System.Private.CoreLib.dll,
        ///     it is sufficient to supply the type name qualified by its namespace.
        /// </summary>
        public string AssemblyQualifiedTypeName { get; }

        /// <summary>
        ///     Creates a new instance of the <see cref="NavigationPropertyInfo"/> class.
        /// </summary>
        /// <param name="propertyInfo">Property information.</param>
        /// <returns>New instance of the <see cref="NavigationPropertyInfo"/> class.</returns>
        public static NavigationPropertyInfo FromPropertyInfo(PropertyInfo? propertyInfo)
        {
            return new NavigationPropertyInfo(
                propertyInfo?.Name,
                propertyInfo?.DeclaringType?.AssemblyQualifiedName);
        }

        /// <summary>
        ///     Converts current model to the <see cref="T:System.Reflection.PropertyInfo"/>.
        /// </summary>
        /// <returns>
        ///     An object representing the public property with the specified name and type.
        /// </returns>
        /// <exception cref="T:Softeq.XToolkit.WhiteLabel.Navigation.PropertyNotFoundException">
        ///     Property with the given name not found on the type with the given name.
        /// </exception>
        public PropertyInfo ToPropertyInfo()
        {
            Type? type;
            PropertyInfo? propertyInfo;

            try
            {
                type = Type.GetType(AssemblyQualifiedTypeName);
            }
            catch (Exception ex)
            {
                throw new PropertyNotFoundException("Type load exception", ex);
            }

            if (type == null)
            {
                throw new PropertyNotFoundException(
                    $"Type with the name {AssemblyQualifiedTypeName} not found");
            }

            try
            {
                propertyInfo = type.GetProperty(PropertyName);
            }
            catch (Exception ex)
            {
                throw new PropertyNotFoundException("Property load exception", ex);
            }

            if (propertyInfo == null)
            {
                throw new PropertyNotFoundException(
                    $"Property with the name {PropertyName} not found in the type {AssemblyQualifiedTypeName}");
            }

            return propertyInfo;
        }
    }
}

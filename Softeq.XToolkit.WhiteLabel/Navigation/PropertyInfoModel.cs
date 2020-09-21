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
    public class PropertyInfoModel
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PropertyInfoModel"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="assemblyQualifiedTypeName">
        ///     The assembly-qualified name of the type.
        ///     See <see cref="P:System.Type.AssemblyQualifiedName"/>.
        ///     <para/>
        ///     If the type is in the currently executing assembly or in mscorlib.dll/System.Private.CoreLib.dll,
        ///     it is sufficient to supply the type name qualified by its namespace.
        /// </param>
        public PropertyInfoModel(string propertyName, string assemblyQualifiedTypeName)
        {
            PropertyName = propertyName;
            AssemblyQualifiedTypeName = assemblyQualifiedTypeName;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PropertyInfoModel"/> class.
        /// </summary>
        /// <param name="propertyInfo">Property information.</param>
        public PropertyInfoModel(PropertyInfo propertyInfo)
            : this(propertyInfo.Name, propertyInfo.DeclaringType.AssemblyQualifiedName)
        {
        }

        /// <summary>
        ///     Gets the name of the property.
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
        ///     Converts current model to <see cref="T:System.Reflection.PropertyInfo"/>.
        /// </summary>
        /// <returns>
        ///     <see cref="T:System.Reflection.PropertyInfo"/> instance
        ///     that corresponds to the current model.
        /// </returns>
        public PropertyInfo ToPropertyInfo()
            => Type.GetType(AssemblyQualifiedTypeName).GetProperty(PropertyName);
    }
}

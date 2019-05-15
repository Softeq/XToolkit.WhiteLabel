// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Softeq.XToolkit.Common.Extensions
{
    public static class AssemblyExtensions
    {
        /// <summary>
        ///     Get's the name of the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The assembly's name.</returns>
        public static string GetAssemblyName(this Assembly assembly)
        {
            return assembly.FullName.Remove(assembly.FullName.IndexOf(','));
        }

        /// <summary>
        ///     Gets a collection of the public types defined in this assembly that are visible outside the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>A collection of the public types defined in this assembly that are visible outside the assembly.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<Type> GetExportedTypes(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            return assembly.ExportedTypes;
        }

        /// <summary>
        ///     Returns a value that indicates whether the specified type can be assigned to the current type.
        /// </summary>
        /// <param name="target">The target type</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified type can be assigned to this type; otherwise, false.</returns>
        public static bool IsAssignableFrom(this Type target, Type type)
        {
            return target.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());
        }
    }
}
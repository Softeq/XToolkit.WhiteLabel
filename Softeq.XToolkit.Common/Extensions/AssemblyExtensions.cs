// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Reflection;

namespace Softeq.XToolkit.Common.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:System.Reflection.Assembly"/>.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        ///     Gets the name of the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The assembly's name.</returns>
        public static string GetAssemblyName(this Assembly assembly)
        {
            return assembly.FullName.Remove(assembly.FullName.IndexOf(','));
        }
    }
}

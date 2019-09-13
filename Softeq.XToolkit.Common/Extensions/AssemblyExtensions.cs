// Developed by Softeq Development Corporation
// http://www.softeq.com

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
    }
}
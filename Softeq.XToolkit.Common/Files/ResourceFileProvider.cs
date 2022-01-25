// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

#pragma warning disable SA1611, SA1615

namespace Softeq.XToolkit.Common.Files
{
    /// <summary>
    ///     Represents methods for file operations with Assembly resources.
    /// </summary>
    public class ResourceFileProvider : IFileProvider
    {
        private const string MethodNotSupportedExceptionMessage = "The method call is not relevant for this file provider.";

        private readonly Assembly _assembly;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ResourceFileProvider"/> class.
        /// </summary>
        /// <param name="assembly">Target Assembly that contains resources.</param>
        public ResourceFileProvider(Assembly assembly)
        {
            _assembly = assembly;
        }

        /// <summary>
        ///     Not Supported.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The method call is not relevant for this file provider.</exception>
        public Task ClearFolderAsync(string path)
        {
            throw new NotSupportedException(MethodNotSupportedExceptionMessage);
        }

        /// <summary>
        ///     Not Supported.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The method call is not relevant for this file provider.</exception>
        public Task CopyFileAsync(string srcPath, string dstPath, bool overwrite)
        {
            throw new NotSupportedException(MethodNotSupportedExceptionMessage);
        }

        /// <summary>
        ///     Not Supported.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The method call is not relevant for this file provider.</exception>
        public Task<bool> FileExistsAsync(string path)
        {
            throw new NotSupportedException(MethodNotSupportedExceptionMessage);
        }

        /// <inheritdoc />
        public Task<Stream> GetFileContentAsync(string fileName)
        {
            return Task.Run(() =>
            {
                var resourceNames = _assembly.GetManifestResourceNames();

                var resourcePaths = resourceNames
                    .Where(x => x.EndsWith(fileName, StringComparison.CurrentCultureIgnoreCase))
                    .ToArray();

                if (!resourcePaths.Any())
                {
                    throw new Exception($"Resource ending with {fileName} not found.");
                }

                if (resourcePaths.Length > 1)
                {
                    throw new Exception(
                        $"Multiple resources ending with {fileName} found: {Environment.NewLine}{string.Join(Environment.NewLine, resourcePaths)}");
                }

                return _assembly.GetManifestResourceStream(resourcePaths.Single());
            });
        }

        /// <summary>
        ///     Not Supported.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The method call is not relevant for this file provider.</exception>
        public Task<Stream> OpenFileForWriteAsync(string path)
        {
            throw new NotSupportedException(MethodNotSupportedExceptionMessage);
        }

        /// <summary>
        ///     Not Supported.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The method call is not relevant for this file provider.</exception>
        public Task RemoveFileAsync(string path)
        {
            throw new NotSupportedException(MethodNotSupportedExceptionMessage);
        }

        /// <summary>
        ///     Not Supported.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The method call is not relevant for this file provider.</exception>
        public Task WriteFileAsync(string path, Stream stream)
        {
            throw new NotSupportedException(MethodNotSupportedExceptionMessage);
        }

        /// <inheritdoc />
        public string GetAbsolutePath(string relativePath) => relativePath;
    }
}

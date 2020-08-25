// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Files
{
    public class ResourceStorageProvider : IFileProvider
    {
        private readonly Assembly _assembly;

        public ResourceStorageProvider(Assembly assembly)
        {
            _assembly = assembly;
        }

        /// <summary>
        ///     Not Implemented.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task ClearFolderAsync(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Not Implemented.
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="dstPath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task CopyFileAsync(string srcPath, string dstPath, bool overwrite)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Not Implemented.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<bool> FileExistsAsync(string path)
        {
            throw new NotImplementedException();
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

                if (resourcePaths.Count() > 1)
                {
                    throw new Exception(
                        $"Multiple resources ending with {fileName} found: {Environment.NewLine}{string.Join(Environment.NewLine, resourcePaths)}");
                }

                return _assembly.GetManifestResourceStream(resourcePaths.Single());
            });
        }

        /// <summary>
        ///     Not Implemented.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Stream> OpenFileForWriteAsync(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Not Implemented.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task RemoveFileAsync(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Not Implemented.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task WriteFileAsync(string path, Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}

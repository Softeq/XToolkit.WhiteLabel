// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Files
{
    public class ResourceStorageProvider : IFilesProvider
    {
        private readonly Assembly _assembly;

        public ResourceStorageProvider(Assembly assembly)
        {
            _assembly = assembly;
        }

        /// <summary>
        ///     Not Implemented
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task ClearFolderAsync(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Not Implemented
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newPath"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<string> CopyFileFromAsync(string path, string newPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Not Implemented
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<bool> ExistsAsync(string path)
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
        ///     Not Implemented
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Stream> OpenStreamForWriteAsync(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Not Implemented
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task RemoveAsync(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Not Implemented
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<string> WriteStreamAsync(string str, Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}

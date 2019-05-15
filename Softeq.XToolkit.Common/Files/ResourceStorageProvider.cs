using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.Common.Files
{
    public class ResourceStorageProvider : IFilesProvider
    {
        private readonly Assembly _assembly;

        public ResourceStorageProvider(Assembly assembly)
        {
            _assembly = assembly;
        }

        public Task ClearFolderAsync(string path)
        {
            throw new NotImplementedException();
        }

        public Task<string> CopyFileFromAsync(string path, string newPath)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string path)
        {
            throw new NotImplementedException();
        }

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

        public Task<Stream> OpenStreamForWriteAsync(string path)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string path)
        {
            throw new NotImplementedException();
        }

        public Task<string> WriteStreamAsync(string str, Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}

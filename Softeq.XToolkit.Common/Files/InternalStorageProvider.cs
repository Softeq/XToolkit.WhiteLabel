using System;
using System.IO;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.Common.Files
{
    public class InternalStorageProvider : IFilesProvider
    {
        private readonly string _rootFolderPath;
        private readonly BaseFileProvider _storageProvider;

        public InternalStorageProvider()
        {
            _rootFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _storageProvider = new BaseFileProvider();
        }

        public Task ClearFolderAsync(string path)
        {
            return _storageProvider.ClearFolderAsync(Path.Combine(_rootFolderPath, path));
        }

        public Task<string> CopyFileFromAsync(string path, string newPath)
        {
            return _storageProvider.CopyFileFromAsync(Path.Combine(_rootFolderPath, path), Path.Combine(_rootFolderPath, newPath));
        }

        public Task<bool> ExistsAsync(string path)
        {
            return _storageProvider.ExistsAsync(Path.Combine(_rootFolderPath, path));
        }

        public Task<Stream> GetFileContentAsync(string path)
        {
            return _storageProvider.GetFileContentAsync(Path.Combine(_rootFolderPath, path));
        }

        public Task<Stream> OpenStreamForWriteAsync(string path)
        {
            return _storageProvider.OpenStreamForWriteAsync(Path.Combine(_rootFolderPath, path));
        }

        public Task<string> WriteStreamAsync(string path, Stream stream)
        {
            return _storageProvider.WriteStreamAsync(Path.Combine(_rootFolderPath, path), stream);
        }

        public Task RemoveAsync(string path)
        {
            return _storageProvider.RemoveAsync(Path.Combine(_rootFolderPath, path));
        }
    }
}
﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Data;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Files
{
    public class BaseFileProvider : IFileProvider
    {
        private readonly IFileSystem _fileSystem;

        public BaseFileProvider(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <inheritdoc />
        public Task ClearFolderAsync(string path)
        {
            path = BuildPath(path);
            return Task.Run(async () =>
            {
                var tasks = new DirectoryInfo(path).GetFiles()
                        .Select(async x => await RemoveFileAsync(x.FullName).ConfigureAwait(false));
                await Task.WhenAll(tasks);
            });
        }

        /// <inheritdoc />
        public Task<Stream> OpenFileForWriteAsync(string path)
        {
            path = BuildPath(path);
            return Task.Run(() => _fileSystem.File.OpenWrite(path));
        }

        /// <inheritdoc />
        public Task RemoveFileAsync(string path)
        {
            path = BuildPath(path);
            return Task.Run(() => _fileSystem.File.Delete(path));
        }

        /// <inheritdoc />
        public Task<Stream> GetFileContentAsync(string path)
        {
            path = BuildPath(path);
            return Task.Run(() => _fileSystem.File.OpenRead(path));
        }

        /// <inheritdoc />
        public Task CopyFileAsync(string srcPath, string dstPath, bool overwrite)
        {
            srcPath = BuildPath(srcPath);
            dstPath = BuildPath(dstPath);
            return Task.Run(() => _fileSystem.File.Copy(srcPath, dstPath, overwrite));
        }

        /// <inheritdoc />
        public async Task WriteFileAsync(string path, Stream stream)
        {
            path = BuildPath(path);
            using (var outputStream = await OpenFileForWriteAsync(path))
            {
                stream.Position = 0;
                await stream.CopyToAsync(outputStream);

                stream.Dispose();
            }
        }

        /// <inheritdoc />
        public Task<bool> FileExistsAsync(string path)
        {
            path = BuildPath(path);
            return Task.Run(() => _fileSystem.File.Exists(path));
        }

        protected virtual string BuildPath(string path) => path;
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Files.Abstractions;

namespace Softeq.XToolkit.Common.Files
{
    /// <summary>
    ///     The provider contains base methods to manipulate files in the target file system.
    /// </summary>
    public class BaseFileProvider : IFileProvider
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseFileProvider"/> class.
        /// </summary>
        /// <param name="fileSystem">Target file system.</param>
        public BaseFileProvider(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <inheritdoc />
        public Task ClearFolderAsync(string path)
        {
            path = GetAbsolutePath(path);
            return Task.Run(async () =>
            {
                var tasks = _fileSystem.Directory.GetFiles(path).
                    Select(async x => await RemoveFileAsync(x).ConfigureAwait(false));
                await Task.WhenAll(tasks).ConfigureAwait(false);
            });
        }

        /// <inheritdoc />
        public Task<Stream> OpenFileForWriteAsync(string path)
        {
            path = GetAbsolutePath(path);
            return Task.Run(() => _fileSystem.File.OpenWrite(path));
        }

        /// <inheritdoc />
        public Task RemoveFileAsync(string path)
        {
            path = GetAbsolutePath(path);
            return Task.Run(() => _fileSystem.File.Delete(path));
        }

        /// <inheritdoc />
        public Task<Stream> GetFileContentAsync(string path)
        {
            path = GetAbsolutePath(path);
            return Task.Run(() => _fileSystem.File.OpenRead(path));
        }

        /// <inheritdoc />
        public Task CopyFileAsync(string sourcePath, string destinationPath, bool overwrite)
        {
            sourcePath = GetAbsolutePath(sourcePath);
            destinationPath = GetAbsolutePath(destinationPath);
            return Task.Run(() => _fileSystem.File.Copy(sourcePath, destinationPath, overwrite));
        }

        /// <inheritdoc />
        public async Task WriteFileAsync(string path, Stream stream)
        {
            path = GetAbsolutePath(path);
            using (var outputStream = await OpenFileForWriteAsync(path).ConfigureAwait(false))
            {
                stream.Position = 0;
                await stream.CopyToAsync(outputStream).ConfigureAwait(false);

                stream.Dispose();
            }
        }

        /// <inheritdoc />
        public Task<bool> FileExistsAsync(string path)
        {
            path = GetAbsolutePath(path);
            return Task.Run(() => _fileSystem.File.Exists(path));
        }

        /// <inheritdoc />
        public virtual string GetAbsolutePath(string relativePath) => relativePath;
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Files
{
    public class BaseFileProvider : IFilesProvider
    {
        /// <inheritdoc />
        public Task ClearFolderAsync(string path)
        {
            return Task.Run(async () =>
            {
                if (Directory.Exists(path))
                {
                    var tasks = new DirectoryInfo(path).GetFiles()
                        .Select(async x => await RemoveAsync(x.FullName).ConfigureAwait(false));
                    await Task.WhenAll(tasks);
                }
            });
        }

        /// <inheritdoc />
        public Task<Stream> OpenStreamForWriteAsync(string path)
        {
            return Task.Run(() =>
            {
                var fileStream = File.OpenWrite(path);
                return (Stream) fileStream;
            });
        }

        /// <inheritdoc />
        public Task RemoveAsync(string path)
        {
            return Task.Run(() =>
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            });
        }

        /// <inheritdoc />
        public Task<Stream> GetFileContentAsync(string path)
        {
            return Task.Run(() =>
            {
                var fileStream = File.OpenRead(path);
                return (Stream) fileStream;
            });
        }

        /// <inheritdoc />
        public Task<string> CopyFileFromAsync(string path, string newPath)
        {
            return Task.Run(() =>
            {
                if (File.Exists(path))
                {
                    File.Copy(path, newPath);

                    return newPath;
                }

                return null;
            });
        }

        /// <inheritdoc />
        public async Task<string> WriteStreamAsync(string path, Stream stream)
        {
            using (var outputStream = await OpenStreamForWriteAsync(path))
            {
                stream.Position = 0;
                await stream.CopyToAsync(outputStream);

                stream.Dispose();
            }

            return path;
        }

        /// <inheritdoc />
        public Task<bool> ExistsAsync(string path)
        {
            return Task.Run(() => File.Exists(path));
        }
    }
}

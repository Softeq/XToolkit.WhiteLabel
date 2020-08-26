// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Files
{
    /// <summary>
    ///     Represents methods for file operations.
    /// </summary>
    public interface IFileProvider
    {
        /// <summary>
        ///     Gets data stream from file with the specified path.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <returns>Data stream.</returns>
        Task<Stream> GetFileContentAsync(string path);

        /// <summary>
        ///     Copies file asynchronously to the new path.
        /// </summary>
        /// <param name="sourcePath">Source file path.</param>
        /// <param name="destinationPath">Destination file path.</param>
        /// <param name="overwrite">Overwrite destination file if exists.</param>
        /// <returns>Copy file Task.</returns>
        Task CopyFileAsync(string sourcePath, string destinationPath, bool overwrite);

        /// <summary>
        ///     Deletes all files from the specified directory.
        /// </summary>
        /// <param name="path">Directory path.</param>
        /// <returns>Clear folder Task.</returns>
        Task ClearFolderAsync(string path);

        /// <summary>
        ///     Deletes the specified file.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <returns>Remove file Task.</returns>
        Task RemoveFileAsync(string path);

        /// <summary>
        ///     Opens an existing file or creates a new file for writing.
        /// </summary>
        /// <param name="path">The file to be opened for writing.</param>
        /// <returns>An unshared Stream object on the specified path with Write access.</returns>
        Task<Stream> OpenFileForWriteAsync(string path);

        /// <summary>
        ///     Checks if file exists.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <returns>True if file exists, otherwise False.</returns>
        Task<bool> FileExistsAsync(string path);

        /// <summary>
        ///     Opens an existing file or creates a new file for writing, and writes bytes to this file.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <param name="stream">Bytes to write.</param>
        /// <returns>Write file Task.</returns>
        Task WriteFileAsync(string path, Stream stream);
    }
}

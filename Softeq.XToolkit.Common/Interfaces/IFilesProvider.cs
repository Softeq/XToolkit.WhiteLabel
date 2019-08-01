// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Interfaces
{
    /// <summary>
    ///     Represent methods for file operations.
    /// </summary>
    public interface IFilesProvider
    {
        /// <summary>
        ///     Get stream from file with specified path
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Returns a Stream</returns>
        Task<Stream> GetFileContentAsync(string path);

        /// <summary>
        ///     Copy file asynchronously to new path
        /// </summary>
        /// <param name="path">Source file path</param>
        /// <param name="newPath">Destination file path</param>
        /// <returns>Return destination path if file copied, overwise null</returns>
        Task<string> CopyFileFromAsync(string path, string newPath);

        /// <summary>
        ///     Delete all files from directory
        /// </summary>
        /// <param name="path">Path for directory</param>
        /// <returns></returns>
        Task ClearFolderAsync(string path);

        /// <summary>
        ///     Delete file if existed
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns></returns>
        Task RemoveAsync(string path);

        /// <summary>
        ///     Opens an existing file or creates a new file for writing.
        /// </summary>
        /// <param name="path">The file to be opened for writing.</param>
        /// <returns>An unshared Stream object on the specified path with Write access.</returns>
        Task<Stream> OpenStreamForWriteAsync(string path);

        /// <summary>
        ///     Check if file existed
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>true if file existed, overwise false</returns>
        Task<bool> ExistsAsync(string path);

        /// <summary>
        ///     Opens an existing file or creates a new file for writing, and write bytes to file
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="stream">Bytes to write</param>
        /// <returns>File path</returns>
        Task<string> WriteStreamAsync(string path, Stream stream);
    }
}

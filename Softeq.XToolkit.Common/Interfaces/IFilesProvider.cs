// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Interfaces
{
    public interface IFilesProvider
    {
        Task<Stream> GetFileContentAsync(string path);
        Task<string> CopyFileFromAsync(string path, string newPath);
        Task ClearFolderAsync(string path);
		Task RemoveAsync(string path);
        Task<Stream> OpenStreamForWriteAsync(string path);
		Task<bool> ExistsAsync(string path);
        Task<string> WriteStreamAsync(string str, Stream stream);
    }
}
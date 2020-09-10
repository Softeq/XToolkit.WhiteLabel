// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Files.Abstractions
{
    public interface IFileSystem
    {
        public IFile File { get; }

        public IDirectory Directory { get; }
    }

    public class DefaultFileSystem : IFileSystem
    {
        public DefaultFileSystem()
        {
            File = new DefaultFile();
            Directory = new DefaultDirectory();
        }

        public IFile File { get; }

        public IDirectory Directory { get; }
    }
}

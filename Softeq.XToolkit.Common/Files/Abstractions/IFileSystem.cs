// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Files.Abstractions
{
    /// <summary>
    ///     The interface of some System.IO primitives.
    /// </summary>
    public interface IFileSystem
    {
        /// <inheritdoc cref="IFile"/>.
        public IFile File { get; }

        /// <inheritdoc cref="IDirectory"/>.
        public IDirectory Directory { get; }
    }

    /// <summary>
    ///     System.IO implementation of <see cref="IFileSystem"/>.
    /// </summary>
    public class DefaultFileSystem : IFileSystem
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultFileSystem"/> class with default implementations.
        /// </summary>
        public DefaultFileSystem()
        {
            File = new DefaultFile();
            Directory = new DefaultDirectory();
        }

        /// <inheritdoc />
        public IFile File { get; }

        /// <inheritdoc />
        public IDirectory Directory { get; }
    }
}

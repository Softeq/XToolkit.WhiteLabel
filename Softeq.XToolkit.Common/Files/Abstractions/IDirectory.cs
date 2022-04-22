// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;

namespace Softeq.XToolkit.Common.Files.Abstractions
{
    /// <summary>
    ///     The interface of <see cref="T:System.IO.Directory"/>.
    /// </summary>
    public interface IDirectory
    {
        /// <inheritdoc cref="Directory.GetFiles(string)"/>
        string[] GetFiles(string path);

        /// <inheritdoc cref="Directory.Exists(string)"/>
        bool Exists(string path);
    }

    /// <summary>
    ///     System.IO implementation of <see cref="IDirectory"/>.
    /// </summary>
    public class DefaultDirectory : IDirectory
    {
        /// <inheritdoc cref="Directory.GetFiles(string)"/>
        public string[] GetFiles(string path) => Directory.GetFiles(path);

        /// <inheritdoc cref="Directory.Exists(string)"/>
        public bool Exists(string path) => Directory.Exists(path);
    }
}

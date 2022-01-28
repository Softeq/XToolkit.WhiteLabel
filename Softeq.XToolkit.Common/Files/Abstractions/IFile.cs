// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;

namespace Softeq.XToolkit.Common.Files.Abstractions
{
    /// <summary>
    ///     The interface of <see cref="T:System.IO.File"/>.
    /// </summary>
    public interface IFile
    {
        /// <inheritdoc cref="File.OpenWrite"/>
        Stream OpenWrite(string path);

        /// <inheritdoc cref="File.OpenRead"/>
        Stream OpenRead(string path);

        /// <inheritdoc cref="File.Delete"/>
        void Delete(string path);

        /// <inheritdoc cref="File.Exists"/>
        bool Exists(string path);

        /// <inheritdoc cref="File.Copy(string,string,bool)"/>
        void Copy(string sourcePath, string destinationPath, bool overwrite);

        /// <inheritdoc cref="File.ReadAllText(string)"/>
        string ReadAllText(string path);
    }

    /// <summary>
    ///     System.IO implementation of <see cref="IFile"/>.
    /// </summary>
    public class DefaultFile : IFile
    {
        /// <inheritdoc cref="T:System.IO.File.OpenWrite"/>
        public Stream OpenWrite(string path) => File.OpenWrite(path);

        /// <inheritdoc cref="T:System.IO.File.OpenRead"/>
        public Stream OpenRead(string path) => File.OpenRead(path);

        /// <inheritdoc cref="T:System.IO.File.Delete"/>
        public void Delete(string path) => File.Delete(path);

        /// <inheritdoc cref="T:System.IO.File.Exists"/>
        public bool Exists(string path) => File.Exists(path);

        /// <inheritdoc cref="T:System.IO.File.Copy(string,string,bool)"/>
        public void Copy(string sourcePath, string destinationPath, bool overwrite)
            => File.Copy(sourcePath, destinationPath, overwrite);

        /// <inheritdoc cref="T:System.IO.File.ReadAllText(string)"/>
        public string ReadAllText(string path) => File.ReadAllText(path);
    }
}

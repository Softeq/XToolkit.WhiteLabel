// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;

namespace Softeq.XToolkit.Common.Files.Abstractions
{
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

    public class DefaultFile : IFile
    {
        public Stream OpenWrite(string path) => File.OpenWrite(path);

        public Stream OpenRead(string path) => File.OpenRead(path);

        public void Delete(string path) => File.Delete(path);

        public bool Exists(string path) => File.Exists(path);

        public void Copy(string sourcePath, string destinationPath, bool overwrite)
            => File.Copy(sourcePath, destinationPath, overwrite);

        public string ReadAllText(string path) => File.ReadAllText(path);
    }
}

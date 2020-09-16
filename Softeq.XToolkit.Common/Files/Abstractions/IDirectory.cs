// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;

namespace Softeq.XToolkit.Common.Files.Abstractions
{
    public interface IDirectory
    {
        /// <inheritdoc cref="Directory.GetFiles(string)"/>
        string[] GetFiles(string path);

        /// <inheritdoc cref="Directory.Exists(string)"/>
        bool Exists(string path);
    }

    public class DefaultDirectory : IDirectory
    {
        public string[] GetFiles(string path) => Directory.GetFiles(path);

        public bool Exists(string path) => Directory.Exists(path);
    }
}

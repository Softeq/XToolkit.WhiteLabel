// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using Softeq.XToolkit.Common.Files.Abstractions;

namespace Softeq.XToolkit.Common.Files
{
    public class InternalStorageFileProvider : BaseFileProvider
    {
        private readonly string _rootFolderPath;

        public InternalStorageFileProvider()
            : base(new DefaultFileSystem())
        {
            _rootFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }

        /// <inheritdoc />
        public override string GetAbsolutePath(string relativePath) => Path.Combine(_rootFolderPath, relativePath);
    }
}

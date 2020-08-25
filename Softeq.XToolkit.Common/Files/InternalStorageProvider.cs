// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using System.IO.Abstractions;

namespace Softeq.XToolkit.Common.Files
{
    public class InternalStorageProvider : BaseFileProvider
    {
        private readonly string _rootFolderPath;

        public InternalStorageProvider()
            : base(new FileSystem())
        {
            _rootFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }

        protected override string BuildPath(string path) => Path.Combine(_rootFolderPath, path);
    }
}

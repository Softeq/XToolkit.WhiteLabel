// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using Softeq.XToolkit.Common.Files.Abstractions;
using IDirectory = Softeq.XToolkit.Common.Files.Abstractions.IDirectory;
using IFile = Softeq.XToolkit.Common.Files.Abstractions.IFile;

namespace Softeq.XToolkit.Common.Tests.Files.BaseFileProviderTests
{
    internal class MockFileSystemWrapper : MockFileSystem, IFileSystem
    {
        public MockFileSystemWrapper(IDictionary<string, MockFileData> files, string currentDirectory = "")
            : base(files, currentDirectory)
        {
            File = new MockFileWrapper(this);
            Directory = new MockDirectoryWrapper(this, currentDirectory);
        }

        public new IFile File { get; }
        public new IDirectory Directory { get; }
    }

    internal class MockFileWrapper : MockFile, IFile
    {
        public MockFileWrapper(IMockFileDataAccessor mockFileDataAccessor)
            : base(mockFileDataAccessor)
        {
        }
    }

    internal class MockDirectoryWrapper : MockDirectory, IDirectory
    {
        public MockDirectoryWrapper(IMockFileDataAccessor mockFileDataAccessor, string currentDirectory)
            : base(mockFileDataAccessor, currentDirectory)
        {
        }
    }
}

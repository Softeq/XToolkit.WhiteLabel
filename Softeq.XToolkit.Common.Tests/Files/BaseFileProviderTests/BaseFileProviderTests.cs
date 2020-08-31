// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Text;
using Softeq.XToolkit.Common.Files;
using Xunit;

namespace Softeq.XToolkit.Common.Tests
{
    public class BaseFileProviderTests
    {
        private const string DirectoryPath1 = @"test_dir/";
        private const string DirectoryPath2 = @"test_dir2/";

        private const string FilePath1 = @"file1.txt";
        private const string FilePath2 = DirectoryPath1 + @"file2.txt";
        private const string FilePath3 = DirectoryPath1 + @"file3.txt";

        private const string MissingDirectoryPath1 = @"test_dir3/";

        private const string MissingFilePath1 = DirectoryPath1 + @"file4.txt";
        private const string MissingFilePath2 = DirectoryPath1 + @"file3";
        private const string MissingFilePath3 = @"some_file.txt";

        private readonly MockFileSystem _mockFileSystem;
        private readonly BaseFileProvider _fileProvider;

        public BaseFileProviderTests()
        {
            _mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { DirectoryPath1, new MockDirectoryData() },
                { DirectoryPath2, new MockDirectoryData() },
                { FilePath1, new MockFileData("Testing is meh") },
                { FilePath2, new MockFileData("I hate testing") },
                { FilePath3, new MockFileData("Testing sucks") }
            });

            _fileProvider = new BaseFileProvider(_mockFileSystem);
        }

        #region FileExists

        [Theory]
        [InlineData(FilePath1)]
        [InlineData(FilePath2)]
        public async void FileExistsAsync_ExistingPath_ReturnsTrue(string path)
        {
            Assert.True(_mockFileSystem.File.Exists(path));

            var result = await _fileProvider.FileExistsAsync(path);

            Assert.True(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(MissingFilePath1)]
        [InlineData(MissingFilePath2)]
        [InlineData(DirectoryPath1)]
        [InlineData(MissingDirectoryPath1)]
        public async void FileExistsAsync_MissingPath_ReturnsFalse(string path)
        {
            Assert.False(_mockFileSystem.File.Exists(path));

            var result = await _fileProvider.FileExistsAsync(path);

            Assert.False(result);
        }

        [Theory]
        [InlineData("")]
        public async void FileExistsAsync_InvalidPath_ThrowsException(string path)
        {
            await Assert.ThrowsAnyAsync<Exception>(async () => await _fileProvider.FileExistsAsync(path));
        }

        #endregion

        #region RemoveFile

        [Theory]
        [InlineData(FilePath1)]
        [InlineData(FilePath2)]
        [InlineData(MissingFilePath1)]
        [InlineData(MissingFilePath2)]
        [InlineData(DirectoryPath1)]
        public async void RemoveFileAsync_ValidPath_RemovesFile(string path)
        {
            await _fileProvider.RemoveFileAsync(path);

            Assert.False(_mockFileSystem.File.Exists(path));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(MissingDirectoryPath1)]
        public async void RemoveFileAsync_InvalidPath_ThrowsException(string path)
        {
            await Assert.ThrowsAnyAsync<Exception>(async () => await _fileProvider.RemoveFileAsync(path));
        }

        #endregion

        #region CopyFile

        [Theory]
        [InlineData(FilePath1, FilePath2)]
        [InlineData(FilePath2, FilePath3)]
        public async void CopyFileAsync_ExistingPathToExistingPath_WithOverwrite_CopiesFile(
            string srcPath, string dstPath)
        {
            Assert.True(_mockFileSystem.File.Exists(dstPath));

            await _fileProvider.CopyFileAsync(srcPath, dstPath, true);

            Assert.True(_mockFileSystem.File.Exists(srcPath));
            Assert.True(_mockFileSystem.File.Exists(dstPath));
        }

        [Theory]
        [InlineData(FilePath1, FilePath2)]
        [InlineData(FilePath2, FilePath3)]
        public async void CopyFileAsync_ExistingPathToExistingPath_WithoutOverwrite_ThrowsException(
            string srcPath, string dstPath)
        {
            Assert.True(_mockFileSystem.File.Exists(dstPath));
            await Assert.ThrowsAnyAsync<Exception>(async () => await _fileProvider.CopyFileAsync(srcPath, dstPath, false));
        }

        [Theory]
        [InlineData(FilePath1, MissingFilePath1, true)]
        [InlineData(FilePath1, MissingFilePath1, false)]
        [InlineData(FilePath2, MissingFilePath2, true)]
        [InlineData(FilePath2, MissingFilePath2, false)]
        public async void CopyFileAsync_ExistingPathToMissingPath_CopiesFile(
            string srcPath, string dstPath, bool overwrite)
        {
            Assert.False(_mockFileSystem.File.Exists(dstPath));

            await _fileProvider.CopyFileAsync(srcPath, dstPath, overwrite);

            Assert.True(_mockFileSystem.File.Exists(srcPath));
            Assert.True(_mockFileSystem.File.Exists(dstPath));
        }

        [Theory]
        [InlineData(FilePath1, FilePath1, true)]
        [InlineData(FilePath1, FilePath1, false)]
        public async void CopyFileAsync_ExistingPathToSamePath_ThrowsException(
            string srcPath, string dstPath, bool overwrite)
        {
            await Assert.ThrowsAnyAsync<Exception>(async () => await _fileProvider.CopyFileAsync(srcPath, dstPath, overwrite));
        }

        [Theory]
        [InlineData(null, FilePath1, true)]
        [InlineData(null, MissingFilePath1, true)]
        [InlineData(null, null, false)]
        [InlineData(null, "", true)]
        [InlineData(MissingFilePath1, FilePath1, true)]
        [InlineData(MissingFilePath1, MissingFilePath2, false)]
        [InlineData(MissingFilePath1, MissingFilePath1, true)]
        [InlineData(MissingFilePath1, null, false)]
        [InlineData(MissingFilePath1, "", true)]
        [InlineData("", FilePath1, false)]
        [InlineData("", MissingFilePath1, true)]
        [InlineData("", "", true)]
        [InlineData("", null, true)]
        public async void CopyFileAsync_MissingOrInvalidPath_ThrowsException(
            string srcPath, string dstPath, bool overwrite)
        {
            await Assert.ThrowsAnyAsync<Exception>(async () => await _fileProvider.CopyFileAsync(srcPath, dstPath, overwrite));
        }

        #endregion

        #region GetFileContent

        [Theory]
        [InlineData(MissingFilePath1)]
        [InlineData(MissingFilePath2)]
        [InlineData("")]
        [InlineData(null)]
        public async void GetFileContentAsync_MissingOrInvalidPath_ThrowsException(string path)
        {
            await Assert.ThrowsAnyAsync<Exception>(async () => await _fileProvider.GetFileContentAsync(path));
        }

        [Theory]
        [InlineData(FilePath1)]
        [InlineData(FilePath3)]
        public async void GetFileContentAsync_ExistingPath_ReturnsValidStream(string path)
        {
            Assert.True(_mockFileSystem.File.Exists(path));

            var stream = await _fileProvider.GetFileContentAsync(path);

            Assert.NotNull(stream);
        }

        #endregion

        #region OpenWrite

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async void OpenFileForWriteAsync_InvalidPath_ThrowsException(string path)
        {
            await Assert.ThrowsAnyAsync<Exception>(async () => await _fileProvider.OpenFileForWriteAsync(path));
        }

        [Theory]
        [InlineData(MissingFilePath1)]
        [InlineData(MissingFilePath3)]
        public async void OpenFileForWriteAsync_MissingPath_CreatesFileAndReturnsValidStream(string path)
        {
            Assert.False(_mockFileSystem.File.Exists(path));

            var stream = await _fileProvider.OpenFileForWriteAsync(path);

            Assert.True(_mockFileSystem.File.Exists(path));
            Assert.NotNull(stream);
        }

        [Theory]
        [InlineData(FilePath2)]
        [InlineData(FilePath3)]
        public async void OpenFileForWriteAsync_ExistingPath_ReturnsValidStream(string path)
        {
            Assert.True(_mockFileSystem.File.Exists(path));

            var stream = await _fileProvider.OpenFileForWriteAsync(path);

            Assert.NotNull(stream);
        }

        #endregion

        #region ClearFolder

        [Theory]
        [InlineData(MissingFilePath2)]
        [InlineData(MissingFilePath3)]
        [InlineData("")]
        [InlineData(null)]
        public async void ClearFolderAsync_MissingOrInvalidPath_ThrowsException(string path)
        {
            await Assert.ThrowsAnyAsync<Exception>(async () => await _fileProvider.ClearFolderAsync(path));
        }

        [Theory]
        [InlineData(DirectoryPath1)]
        public async void ClearFolderAsync_ExistingFolderPath_ClearsFolder(string path)
        {
            Assert.True(_mockFileSystem.Directory.Exists(path));
            Assert.NotEmpty(_mockFileSystem.Directory.GetFiles(path));

            await _fileProvider.ClearFolderAsync(path);

            Assert.True(_mockFileSystem.Directory.Exists(path));
            Assert.Empty(_mockFileSystem.Directory.GetFiles(path));
        }

        #endregion

        #region WriteFile

        [Theory]
        [InlineData(FilePath2, "content")]
        [InlineData(FilePath3, "content")]
        public async void WriteFileAsync_ExistingPath_WritesToFile(
            string path, string content)
        {
            Assert.True(_mockFileSystem.File.Exists(path));

            using var stream = CreateStream(content);
            await _fileProvider.WriteFileAsync(path, stream);

            var fileContent = _mockFileSystem.File.ReadAllText(path);
            Assert.StartsWith(content, fileContent);
        }

        [Theory]
        [InlineData(MissingFilePath2, "content")]
        [InlineData(MissingFilePath3, "content")]
        public async void WriteFileAsync_MissingPath_CreatesFileAndWritesToFile(
            string path, string content)
        {
            Assert.False(_mockFileSystem.File.Exists(path));

            using var stream = CreateStream(content);
            await _fileProvider.WriteFileAsync(path, stream);

            Assert.True(_mockFileSystem.File.Exists(path));
            var fileContent = _mockFileSystem.File.ReadAllText(path);
            Assert.Equal(content, fileContent);
        }

        [Theory]
        [InlineData(null, "content")]
        [InlineData("", "content")]
        public async void WriteFileAsync_InvalidPath_ThrowsException(
            string path, string content)
        {
            using var stream = CreateStream(content);

            await Assert.ThrowsAnyAsync<Exception>(async () => await _fileProvider.WriteFileAsync(path, stream));
        }

        private Stream CreateStream(string content)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(content);
            return new MemoryStream(byteArray);
        }

        #endregion

        #region GetAbsolutePath

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(FilePath1)]
        public void GetAbsolutePath_WhenCalled_ReturnsSamePath(string relativePath)
        {
            var absolutePath = _fileProvider.GetAbsolutePath(relativePath);

            Assert.Equal(relativePath, absolutePath);
        }

        #endregion
    }
}

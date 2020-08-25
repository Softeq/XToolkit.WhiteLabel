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
        private const string DirectoryPath1 = @"test_dir\";
        private const string DirectoryPath2 = @"test_dir2\";

        private const string FilePath1 = @"file1.txt";
        private const string FilePath2 = DirectoryPath1 + @"file2.txt";
        private const string FilePath3 = DirectoryPath1 + @"file3.txt";

        private const string MissingDirectoryPath1 = @"test_dir3\";

        private const string MissingFilePath1 = DirectoryPath1 + @"file4.txt";
        private const string MissingFilePath2 = DirectoryPath1 + @"file3";
        private const string MissingFilePath3 = @"some_file.txt";

        private readonly MockFileSystem _mockFileSystem;
        private readonly IFileProvider _filesProvider;

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

            _filesProvider = new BaseFileProvider(_mockFileSystem);
        }

        #region FileExists

        [Theory]
        [InlineData(FilePath1)]
        [InlineData(FilePath2)]
        public async void FileExistsAsync_ExistingPath_ReturnsTrue(string path)
        {
            Assert.True(_mockFileSystem.FileExists(path));
            var result = await _filesProvider.FileExistsAsync(path);
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
            var result = await _filesProvider.FileExistsAsync(path);

            Assert.False(result);
        }

        [Theory]
        [InlineData("")]
        public async void FileExistsAsync_InvalidPath_ThrowsException(string path)
        {
            await Assert.ThrowsAnyAsync<Exception>(async () => await _filesProvider.FileExistsAsync(path));
        }

        #endregion

        #region RemoveFile

        [Theory]
        [InlineData(FilePath1)]
        [InlineData(FilePath2)]
        [InlineData(MissingFilePath1)]
        [InlineData(MissingFilePath2)]
        [InlineData(DirectoryPath1)]
        [InlineData(MissingDirectoryPath1)]
        public async void RemoveFileAsync_ValidPath_RemovesFile(string path)
        {
            await _filesProvider.RemoveFileAsync(path);

            Assert.False(_mockFileSystem.FileExists(path));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async void RemoveFileAsync_InvalidPath_ThrowsException(string path)
        {
            await Assert.ThrowsAnyAsync<Exception>(async () => await _filesProvider.RemoveFileAsync(path));
        }

        #endregion

        #region CopyFile

        [Theory]
        [InlineData(FilePath1, FilePath2)]
        [InlineData(FilePath2, FilePath3)]
        public async void CopyFileAsync_ExistingPathToExistingPath_WithOverwrite_CopiesFile(
            string srcPath, string dstPath)
        {
            Assert.True(_mockFileSystem.FileExists(dstPath));

            await _filesProvider.CopyFileAsync(srcPath, dstPath, true);

            Assert.True(_mockFileSystem.FileExists(srcPath));
            Assert.True(_mockFileSystem.FileExists(dstPath));
        }

        [Theory]
        [InlineData(FilePath1, FilePath2)]
        [InlineData(FilePath2, FilePath3)]
        public async void CopyFileAsync_ExistingPathToExistingPath_WithoutOverwrite_ThrowsException(
            string srcPath, string dstPath)
        {
            Assert.True(_mockFileSystem.FileExists(dstPath));
            await Assert.ThrowsAnyAsync<Exception>(async () => await _filesProvider.CopyFileAsync(srcPath, dstPath, false));
        }

        [Theory]
        [InlineData(FilePath1, MissingFilePath1, true)]
        [InlineData(FilePath1, MissingFilePath1, false)]
        [InlineData(FilePath2, MissingFilePath2, true)]
        [InlineData(FilePath2, MissingFilePath2, false)]
        public async void CopyFileAsync_ExistingPathToMissingPath_CopiesFile(
            string srcPath, string dstPath, bool overwrite)
        {
            Assert.False(_mockFileSystem.FileExists(dstPath));

            await _filesProvider.CopyFileAsync(srcPath, dstPath, overwrite);

            Assert.True(_mockFileSystem.FileExists(srcPath));
            Assert.True(_mockFileSystem.FileExists(dstPath));
        }

        [Theory]
        [InlineData(FilePath1, FilePath1, true)]
        [InlineData(FilePath1, FilePath1, false)]
        public async void CopyFileAsync_ExistingPathToSamePath_ThrowsException(
            string srcPath, string dstPath, bool overwrite)
        {
            await Assert.ThrowsAnyAsync<Exception>(async () => await _filesProvider.CopyFileAsync(srcPath, dstPath, overwrite));
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
            await Assert.ThrowsAnyAsync<Exception>(async () => await _filesProvider.CopyFileAsync(srcPath, dstPath, overwrite));
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
            await Assert.ThrowsAnyAsync<Exception>(async () => await _filesProvider.GetFileContentAsync(path));
        }

        [Theory]
        [InlineData(FilePath1)]
        [InlineData(FilePath3)]
        public async void GetFileContentAsync_ExistingPath_ReturnsValidStream(string path)
        {
            Assert.True(_mockFileSystem.FileExists(path));

            var stream = await _filesProvider.GetFileContentAsync(path);

            Assert.NotNull(stream);
        }

        #endregion

        #region OpenWrite

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async void OpenFileForWriteAsync_InvalidPath_ThrowsException(string path)
        {
            await Assert.ThrowsAnyAsync<Exception>(async () => await _filesProvider.OpenFileForWriteAsync(path));
        }

        [Theory]
        [InlineData(MissingFilePath1)]
        [InlineData(MissingFilePath3)]
        public async void OpenFileForWriteAsync_MissingPath_CreatesFileAndReturnsValidStream(string path)
        {
            Assert.False(_mockFileSystem.FileExists(path));

            var stream = await _filesProvider.OpenFileForWriteAsync(path);

            Assert.True(_mockFileSystem.FileExists(path));
            Assert.NotNull(stream);
        }

        [Theory]
        [InlineData(FilePath2)]
        [InlineData(FilePath3)]
        public async void OpenFileForWriteAsync_ExistingPath_ReturnsValidStream(string path)
        {
            Assert.True(_mockFileSystem.FileExists(path));

            var stream = await _filesProvider.OpenFileForWriteAsync(path);

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
            await Assert.ThrowsAnyAsync<Exception>(async () => await _filesProvider.ClearFolderAsync(path));
        }

        //Does not work for some reason
        [Theory]
        [InlineData(DirectoryPath1)]
        public async void ClearFolderAsync_ExistingFolderPath_ClearsFolder(string path)
        {
            Assert.True(_mockFileSystem.Directory.Exists(path));
            Assert.NotEmpty(_mockFileSystem.Directory.GetFiles(path));

            await _filesProvider.ClearFolderAsync(path);

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
            Assert.True(_mockFileSystem.FileExists(path));

            byte[] byteArray = Encoding.ASCII.GetBytes(content);
            MemoryStream stream = new MemoryStream(byteArray);

            await _filesProvider.WriteFileAsync(path, stream);

            var fileContent = _mockFileSystem.File.ReadAllText(path);
            Assert.StartsWith(content, fileContent);
        }

        [Theory]
        [InlineData(MissingFilePath2, "content")]
        [InlineData(MissingFilePath3, "content")]
        public async void WriteFileAsync_MissingPath_CreatesFileAndWritesToFile(
            string path, string content)
        {
            Assert.False(_mockFileSystem.FileExists(path));

            byte[] byteArray = Encoding.ASCII.GetBytes(content);
            MemoryStream stream = new MemoryStream(byteArray);

            await _filesProvider.WriteFileAsync(path, stream);

            Assert.True(_mockFileSystem.FileExists(path));
            var fileContent = _mockFileSystem.File.ReadAllText(path);
            Assert.Equal(content, fileContent);
        }

        [Theory]
        [InlineData(null, "content")]
        [InlineData("", "content")]
        public async void WriteFileAsync_InvalidPath_ThrowsException(
            string path, string content)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(content);
            MemoryStream stream = new MemoryStream(byteArray);

            await Assert.ThrowsAnyAsync<Exception>(async () => await _filesProvider.WriteFileAsync(path, stream));
        }

        #endregion
    }
}

using NSubstitute;
using Softeq.XToolkit.Common.Interfaces;
using Xunit;

namespace Softeq.XToolkit.Tests.Core.Common
{
    public class FilesProviderTests
    {
        public FilesProviderTests()
        {
            _filesProvider = Substitute.For<IFilesProvider>();
        }

        private readonly IFilesProvider _filesProvider;

        [Fact]
        public async void Fix_inerface()
        {
            //arrange
            //act
            await _filesProvider.ClearFolderAsync("test");
            await _filesProvider.CopyFileFromAsync("test", "test1");
            await _filesProvider.RemoveAsync("test");
            await _filesProvider.ExistsAsync("test");
            await _filesProvider.OpenStreamForWriteAsync("test");
            await _filesProvider.GetFileContentAsync("test");
        }
    }
}
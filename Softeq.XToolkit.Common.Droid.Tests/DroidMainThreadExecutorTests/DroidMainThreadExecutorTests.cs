// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Xunit;

namespace Softeq.XToolkit.Common.Droid.Tests.DroidMainThreadExecutorTests
{
    public class DroidMainThreadExecutorTests
    {
        private readonly DroidMainThreadExecutor _executor;

        public DroidMainThreadExecutorTests()
        {
            _executor = new DroidMainThreadExecutor();
        }

        [Fact]
        public async Task IsMainThread_InMainThread_ReturnsTrue()
        {
            var result = await Helpers.RunOnUIThreadAsync(() => _executor.IsMainThread);

            Assert.True(result);
        }

        [Fact]
        public async Task IsMainThread_NotInMainThread_ReturnsFalse()
        {
            var result = await Task.Run(() => _executor.IsMainThread);

            Assert.False(result);
        }
    }
}

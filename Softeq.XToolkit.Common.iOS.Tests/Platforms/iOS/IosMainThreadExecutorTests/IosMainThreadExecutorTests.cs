// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Xunit;

namespace Softeq.XToolkit.Common.iOS.Tests.IosMainThreadExecutorTests;

public class IosMainThreadExecutorTests
{
    private readonly IosMainThreadExecutor _executor;

    public IosMainThreadExecutorTests()
    {
        _executor = new IosMainThreadExecutor();
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

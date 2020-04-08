// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public partial class AsyncCommandGenericTests
    {
        [Fact]
        public async Task ExecuteAsync_CalledOneTime_ExecutesOneTime()
        {
            var command = CreateAsyncCommand(_func);

            await command.ExecuteAsync(null);

            await _func.Received(1).Invoke(null);
        }
    }
}

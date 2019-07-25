// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Xunit;

namespace Softeq.XToolkit.Common.Tests
{
    public class TaskReferenceTest
    {
        private Task GetTestTask()
        {
            return Task.Factory.StartNew(() => { Assert.True(true, "method called"); });
        }

        [Fact]
        public async Task Test()
        {
            var taskReference = new TaskReference(GetTestTask);
            await taskReference.RunAsync();
        }
    }
}
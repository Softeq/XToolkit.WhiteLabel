// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Tests.Timers
{
    public class TestFunctionClass
    {
        public int ActionCount { get; set; }

        public Task Function()
        {
            ActionCount++;
            return Task.CompletedTask;
        }
    }
}

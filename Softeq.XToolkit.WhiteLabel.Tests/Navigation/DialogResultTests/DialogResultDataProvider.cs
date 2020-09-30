// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Navigation.DialogResultTests
{
    internal static class DialogResultDataProvider
    {
        private static readonly Task<bool> CompletedTask = Task.FromResult(true);

        public static TheoryData<Task<bool>> CtorData
           => new TheoryData<Task<bool>>
           {
               { null },
               { CompletedTask },
           };

        public static TheoryData<string, Task<bool>> GenericCtorData
           => new TheoryData<string, Task<bool>>
           {
               { null, null },
               { null, CompletedTask },
               { string.Empty, null },
               { string.Empty, CompletedTask },
           };
    }
}

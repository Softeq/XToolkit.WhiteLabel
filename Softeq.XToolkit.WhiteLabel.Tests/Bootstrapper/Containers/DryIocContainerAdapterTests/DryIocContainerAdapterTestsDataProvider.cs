// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Bootstrapper.Containers.DryIocContainerAdapterTests
{
    internal static class DryIocContainerAdapterTestsDataProvider
    {
        public static TheoryData<object[]> ParamsData
           => new TheoryData<object[]>
           {
               { new object[] { } },
               { new object[] { new object() } },
               { new object[] { new object(), new object(), new object() } },
           };
    }
}

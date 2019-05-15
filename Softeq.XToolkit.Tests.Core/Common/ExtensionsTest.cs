// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xunit;

namespace Softeq.XToolkit.Tests.Core.Common
{
    public class ExtensionsTest
    {
        [Fact]
        public void GetAssemblyNameTest()
        {
            var name = GetType().Assembly.GetName().Name;
            Assert.Equal(name, "Softeq.XToolkit.Tests.Core");
        }

        [Fact]
        public void GetExportedTypesTest()
        {
            var types = GetType().Assembly.GetExportedTypes();
            Assert.Contains(typeof(ExtensionsTest), types);
        }

        [Fact]
        public void IsAssignableFromTest()
        {
            Assert.True(typeof(ExtensionsTest).IsAssignableFrom(typeof(ExtensionsTest)));
        }
    }
}
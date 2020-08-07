// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Extensions;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Extensions.AssemblyExtensionsTests
{
    public class AssemblyExtensionsTests
    {
        [Theory]
        [MemberData(nameof(AssemblyExtensionsDataProvider.AssemblyNamesData),
            MemberType = typeof(AssemblyExtensionsDataProvider))]
        public void GetAssemblyName_ForAssembly_ReturnsCorrectName(Type type, string expectedName)
        {
            var assembly = type.Assembly;

            var name = assembly.GetAssemblyName();

            Assert.Equal(expectedName, name);
        }
    }
}

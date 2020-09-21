// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Reflection;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Navigation.PropertyInfoModelTests
{
    internal static class PropertyInfoModelDataProvider
    {
        private static readonly string AssemblyQualifiedTestClassName =
            typeof(TestClass).AssemblyQualifiedName;

        public static TheoryData<PropertyInfo, string, string> PropertyInfoData
           => new TheoryData<PropertyInfo, string, string>
           {
               {
                   typeof(TestClass).GetProperty(nameof(TestClass.TestIntProperty)),
                   nameof(TestClass.TestIntProperty),
                   AssemblyQualifiedTestClassName
               },
               {
                   typeof(TestClass).GetProperty(nameof(TestClass.TestStringProperty)),
                   nameof(TestClass.TestStringProperty),
                   AssemblyQualifiedTestClassName
               },
           };
    }
}

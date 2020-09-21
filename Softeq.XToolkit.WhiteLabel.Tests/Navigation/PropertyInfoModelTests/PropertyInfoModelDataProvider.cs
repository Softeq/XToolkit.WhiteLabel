// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Reflection;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Navigation.PropertyInfoModelTests
{
    internal static class PropertyInfoModelDataProvider
    {
        private const string InvalidClassName = "InvalidClassName";
        private const string InvalidPropertyName = "InvalidPropertyName";

        private static readonly string TestClassName = typeof(TestClass).Name;
        private static readonly string FullTestClassName = typeof(TestClass).FullName;
        private static readonly string AssemblyQualifiedTestClassName =
            typeof(TestClass).AssemblyQualifiedName;
        private static readonly string AssemblyQualifiedWrongTestClassName =
            typeof(TestClass2).AssemblyQualifiedName;

        public static TheoryData<PropertyInfo, string, string> ValidPropertyInfoData
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
               }
           };

        public static TheoryData<string, string> InvalidClassNameTestData
           => new TheoryData<string, string>
           {
               { null, null },
               { nameof(TestClass.TestIntProperty), string.Empty },
               { nameof(TestClass.TestIntProperty), null },
               { nameof(TestClass.TestIntProperty), InvalidClassName },
               { nameof(TestClass.TestIntProperty), TestClassName },
               { nameof(TestClass.TestIntProperty), FullTestClassName },
               { nameof(TestClass.TestIntProperty), AssemblyQualifiedWrongTestClassName }
           };

        public static TheoryData<string, string> InvalidPropertyNameTestData
           => new TheoryData<string, string>
           {
               { null, AssemblyQualifiedTestClassName },
               { string.Empty, AssemblyQualifiedTestClassName },
               { InvalidPropertyName, AssemblyQualifiedTestClassName },
               { nameof(TestClass.TestField), AssemblyQualifiedTestClassName },
               { TestClass.PrivatePropertyName, AssemblyQualifiedTestClassName },
               { TestClass.ProtectedPropertyName, AssemblyQualifiedTestClassName },
               { nameof(TestClass.TestVoidMethod), AssemblyQualifiedTestClassName },
               { nameof(TestClass.TestCharMethod), AssemblyQualifiedTestClassName }
           };
    }
}

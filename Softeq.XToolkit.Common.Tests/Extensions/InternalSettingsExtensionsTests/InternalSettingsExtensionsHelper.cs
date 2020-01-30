// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Tests.Extensions.InternalSettingsExtensionsTests
{
    internal static class InternalSettingsExtensionsHelper
    {
        public const string Key = "test_key";

        public static TestObject NullTestObject => null;
        public static TestObject NotNullTestObject => new TestObject();

        internal class TestObject
        {
            public bool BoolValue { get; set; }
            public int IntValue { get; set; }
            public string StringValue { get; set; }
        }
    }
}

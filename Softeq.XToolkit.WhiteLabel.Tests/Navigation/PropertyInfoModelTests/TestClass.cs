// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Tests.Navigation.PropertyInfoModelTests
{
    public class TestClass2
    {
        public int TestProperty { get; set; }
    }

    public class TestClass
    {
        public const string PrivatePropertyName = nameof(TestPrivateProperty);
        public const string ProtectedPropertyName = nameof(TestProtectedProperty);

        public int TestField;

        private int TestPrivateProperty { get; set; }
        protected int TestProtectedProperty { get; set; }

        public int TestIntProperty { get; set; }
        public string TestStringProperty { get; set; }

        public void TestVoidMethod()
        {
        }

        public char TestCharMethod(string str) => str[0];
    }
}

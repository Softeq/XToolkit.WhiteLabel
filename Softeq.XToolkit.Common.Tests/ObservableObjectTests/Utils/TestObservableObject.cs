// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Tests.ObservableObjectTests.Utils
{
    internal class TestObservableObject : ObservableObject
    {
        private string _testProp;

        public string TestProp
        {
            get => _testProp;
            set => Set(ref _testProp, value);
        }

        public bool SetTestPropWithPropName(string value)
        {
            return Set(nameof(TestProp), ref _testProp, value);
        }

        public bool SetTestPropertyWithPropertyExpression(string value)
        {
            return Set(() => TestProp, ref _testProp, value);
        }

        public static TestObservableObject CreateEmpty()
        {
            return new TestObservableObject();
        }

        public static TestObservableObject CreateWithValue(string value)
        {
            return new TestObservableObject { TestProp = value };
        }
    }
}

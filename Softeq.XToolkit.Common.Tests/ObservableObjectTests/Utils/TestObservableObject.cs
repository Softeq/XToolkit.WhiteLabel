// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Tests.ObservableObjectTests.Utils
{
    internal class TestObservableObject : ObservableObject
    {
        private string _testProperty;

        public string TestProperty
        {
            get => _testProperty;
            set => Set(ref _testProperty, value);
        }

        public bool SetTestPropertyByPropertyName(string value)
        {
            return Set(nameof(TestProperty), ref _testProperty, value);
        }

        public bool SetTestPropertyByPropertyExpression(string value)
        {
            return Set(() => TestProperty, ref _testProperty, value);
        }

        public static TestObservableObject CreateEmpty()
        {
            return new TestObservableObject();
        }

        public static TestObservableObject CreateWithValue(string value)
        {
            return new TestObservableObject { TestProperty = value };
        }
    }
}

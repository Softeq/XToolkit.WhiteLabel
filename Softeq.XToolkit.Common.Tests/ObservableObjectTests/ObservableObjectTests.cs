// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Softeq.XToolkit.Common.Tests.ObservableObjectTests.Utils;
using Xunit;
using Xunit.Sdk;
using static Softeq.XToolkit.Common.Tests.ObservableObjectTests.Utils.CustomPropertyChangedAsserts;

namespace Softeq.XToolkit.Common.Tests.ObservableObjectTests
{
    public class ObservableObjectTests
    {
        private const string TestString = "TestString";

        [Fact]
        public void Constructor_Default_CreatesINotifyPropertyChangedInstance()
        {
            var obj = CreateObservableObject();

            Assert.IsAssignableFrom<INotifyPropertyChanged>(obj);
        }

        [Fact]
        public void Refresh_RaisesPropertyChangedWithEmptyPropertyName()
        {
            var obj = CreateObservableObject();

            Assert_PropertyChanged(obj, string.Empty, () =>
            {
                obj.Refresh();
            });
        }

        [FactInDebugOnly]
        public void RaisePropertyChanged_WithNonExistentPropertyName_ThrowsArgumentException()
        {
            var obj = CreateObservableObject();

            Assert.Throws<ArgumentException>(() => obj.RaisePropertyChanged(TestString));
        }

        [FactInDebugOnly]
        public void RaisePropertyChanged_PropertyExpressionWithoutHandler_ExecutesWasted()
        {
            var obj = CreateObservableObject();

            obj.RaisePropertyChanged(() => PropertyExpressionsProvider.TestProperty);
        }

        [FactInDebugOnly]
        public void RaisePropertyChanged_PropertyExpressionWithHandlerAndNonExistentProperty_ThrowsArgumentException()
        {
            var obj = CreateObservableObjectWithHandler();

            Assert.Throws<ArgumentException>(() =>
            {
                obj.RaisePropertyChanged(() => PropertyExpressionsProvider.TestProperty);
            });
        }

        [FactInDebugOnly]
        public void RaisePropertyChanged_PropertyExpressionWithHandlerAndMethodName_ThrowsArgumentException()
        {
            var obj = CreateObservableObjectWithHandler();

            Assert.Throws<ArgumentException>(() =>
            {
                obj.RaisePropertyChanged(() => PropertyExpressionsProvider.TestMethod());
            });
        }

        [FactInDebugOnly]
        public void RaisePropertyChanged_PropertyExpressionWithHandlerAndEmptyExpression_ThrowsArgumentException()
        {
            var obj = CreateObservableObjectWithHandler();

            Assert.Throws<ArgumentException>(() =>
            {
                obj.RaisePropertyChanged(() => string.Empty);
            });
        }

        [FactInDebugOnly]
        public void RaisePropertyChanged_PropertyExpressionWithHandlerAndNullExpression_ThrowsArgumentNullException()
        {
            var obj = CreateObservableObjectWithHandler();

            Assert.Throws<ArgumentNullException>(() =>
            {
                obj.RaisePropertyChanged(((Expression<Func<string>>)null)!);
            });
        }

        [Fact]
        public void Set_RegularProperty_RisesPropertyChanged()
        {
            var obj = TestObservableObject.CreateEmpty();

            Assert_PropertyChanged(obj, nameof(obj.TestProperty), () =>
            {
                obj.TestProperty = TestString;
            });
        }

        [Fact]
        public void Set_RegularPropertySameValueTwice_NotRisesPropertyChangedSecondTime()
        {
            var obj = TestObservableObject.CreateWithValue(TestString);

            Assert.Throws<PropertyChangedException>(() =>
            {
                Assert_PropertyChanged(obj, nameof(obj.TestProperty), () =>
                {
                    obj.TestProperty = TestString;
                });
            });
        }

        [Fact]
        public void Set_PropertyByPropertyName_RisesPropertyChanged()
        {
            var obj = TestObservableObject.CreateEmpty();

            Assert_PropertyChanged(obj, nameof(obj.TestProperty), () =>
            {
                obj.SetTestPropertyByPropertyName(TestString);
            });
        }

        [Fact]
        public void Set_PropertyByPropertyNameTwice_LastReturnsFalse()
        {
            var obj = TestObservableObject.CreateWithValue(TestString);

            var result = obj.SetTestPropertyByPropertyName(TestString);

            Assert.False(result);
        }

        [Fact]
        public void Set_PropertyByPropertyExpression_RisesPropertyChanged()
        {
            var obj = TestObservableObject.CreateEmpty();

            Assert_PropertyChanged(obj, nameof(obj.TestProperty), () =>
            {
                obj.SetTestPropertyByPropertyExpression(TestString);
            });
        }

        [Fact]
        public void Set_PropertyByPropertyExpressionTwice_LastReturnsFalse()
        {
            var obj = TestObservableObject.CreateWithValue(TestString);

            var result = obj.SetTestPropertyByPropertyExpression(TestString);

            Assert.False(result);
        }

        [FactInDebugOnly]
        public void VerifyPropertyName_NonExistentPropertyName_ThrowsArgumentException()
        {
            var obj = CreateObservableObject();
            var propertyName = TestString;

            Assert.Throws<ArgumentException>(() =>
            {
                obj.VerifyPropertyName(propertyName);
            });
        }

        [TheoryInDebugOnly]
        [InlineData(null)]
        [InlineData("")]
        public void VerifyPropertyName_NullOrEmptyPropertyName_ExecutesWithoutException(string propertyName)
        {
            var obj = CreateObservableObject();

            obj.VerifyPropertyName(propertyName);
        }

        [FactInDebugOnly]
        public void VerifyPropertyName_ExistentPropertyName_ExecutesWithoutException()
        {
            var obj = TestObservableObject.CreateWithValue(TestString);
            var propertyName = nameof(obj.TestProperty);

            obj.VerifyPropertyName(propertyName);
        }

        private static ObservableObject CreateObservableObject()
        {
            return new ObservableObject();
        }

        private static ObservableObject CreateObservableObjectWithHandler()
        {
            var obj = CreateObservableObject();
            obj.PropertyChanged += (sender, args) => { };
            return obj;
        }
    }
}

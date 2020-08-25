// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.ComponentModel;
using System.Diagnostics;
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

        [Fact]
        [Conditional("DEBUG")]
        public void RaisePropertyChanged_WithNonExistentPropertyName_ThrowsArgumentException()
        {
            var obj = CreateObservableObject();

            Assert.Throws<ArgumentException>(() => obj.RaisePropertyChanged(TestString));
        }

        [Fact]
        [Conditional("DEBUG")]
        public void RaisePropertyChanged_PropertyExpressionWithoutHandler_ExecutesWasted()
        {
            var obj = CreateObservableObject();

            obj.RaisePropertyChanged(() => PropertyExpressionsProvider.TestProp);
        }

        [Fact]
        [Conditional("DEBUG")]
        public void RaisePropertyChanged_PropertyExpressionWithHandlerAndNonExistentProperty_ThrowsArgumentException()
        {
            var obj = CreateObservableObject();

            Assert.Throws<ArgumentException>(() =>
            {
                Assert_PropertyChanged(obj, string.Empty, () =>
                {
                    obj.RaisePropertyChanged(() => PropertyExpressionsProvider.TestProp);
                });
            });
        }

        [Fact]
        [Conditional("DEBUG")]
        public void RaisePropertyChanged_PropertyExpressionWithHandlerAndMethodName_ThrowsArgumentException()
        {
            var obj = CreateObservableObject();

            Assert.Throws<ArgumentException>(() =>
            {
                Assert_PropertyChanged(obj, string.Empty, () =>
                {
                    obj.RaisePropertyChanged(() => PropertyExpressionsProvider.TestMethod());
                });
            });
        }

        [Fact]
        [Conditional("DEBUG")]
        public void RaisePropertyChanged_PropertyExpressionWithHandlerAndEmptyExpression_ThrowsArgumentException()
        {
            var obj = CreateObservableObject();

            Assert.Throws<ArgumentException>(() =>
            {
                Assert_PropertyChanged(obj, string.Empty, () =>
                {
                    obj.RaisePropertyChanged(() => string.Empty);
                });
            });
        }

        [Fact]
        [Conditional("DEBUG")]
        public void RaisePropertyChanged_PropertyExpressionWithHandlerAndNullExpression_ThrowsArgumentNullException()
        {
            var obj = CreateObservableObject();

            Assert.Throws<ArgumentNullException>(() =>
            {
                Assert_PropertyChanged(obj, string.Empty, () =>
                {
                    obj.RaisePropertyChanged(((Expression<Func<string>>)null)!);
                });
            });
        }

        [Fact]
        public void Set_RegularProperty_RisesPropertyChanged()
        {
            var obj = TestObservableObject.CreateEmpty();

            Assert_PropertyChanged(obj, nameof(obj.TestProp), () =>
            {
                obj.TestProp = TestString;
            });
        }

        [Fact]
        public void Set_RegularPropertySameValueTwice_NotRisesPropertyChangedSecondTime()
        {
            var obj = TestObservableObject.CreateWithValue(TestString);

            Assert.Throws<PropertyChangedException>(() =>
            {
                Assert_PropertyChanged(obj, nameof(obj.TestProp), () =>
                {
                    obj.TestProp = TestString;
                });
            });
        }

        [Fact]
        public void Set_PropertyByPropertyName_RisesPropertyChanged()
        {
            var obj = TestObservableObject.CreateEmpty();

            Assert_PropertyChanged(obj, nameof(obj.TestProp), () =>
            {
                obj.SetTestPropWithPropName(TestString);
            });
        }

        [Fact]
        public void Set_PropertyByPropertyNameTwice_LastReturnsFalse()
        {
            var obj = TestObservableObject.CreateWithValue(TestString);

            var result = obj.SetTestPropWithPropName(TestString);

            Assert.False(result);
        }

        [Fact]
        public void Set_PropertyByPropertyExpression_RisesPropertyChanged()
        {
            var obj = TestObservableObject.CreateEmpty();

            Assert_PropertyChanged(obj, nameof(obj.TestProp), () =>
            {
                obj.SetTestPropertyWithPropertyExpression(TestString);
            });
        }

        [Fact]
        public void Set_PropertyByPropertyExpressionTwice_LastReturnsFalse()
        {
            var obj = TestObservableObject.CreateWithValue(TestString);

            var result = obj.SetTestPropertyWithPropertyExpression(TestString);

            Assert.False(result);
        }

        private static ObservableObject CreateObservableObject()
        {
            return new ObservableObject();
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Reflection;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Navigation.PropertyInfoModelTests
{
    public class PropertyInfoModelTests
    {
        [Theory]
        [PairwiseData]
        public void Ctor_WithPropertyAndTypeNames_InitializesProperties(
            [CombinatorialValues("", "abc")] string propertyName,
            [CombinatorialValues("", "abc")] string typeName)
        {
            var model = new NavigationPropertyInfo(propertyName, typeName);

            Assert.Equal(propertyName, model.PropertyName);
            Assert.Equal(typeName, model.AssemblyQualifiedTypeName);
        }

        [Fact]
        public void Ctor_WithNullArgs_InitializesPropertiesToEmptyStrings()
        {
            var model = new NavigationPropertyInfo(null, null);

            Assert.Empty(model.PropertyName);
            Assert.Empty(model.AssemblyQualifiedTypeName);
        }

        [Theory]
        [MemberData(
            nameof(PropertyInfoModelDataProvider.ValidPropertyInfoData),
            MemberType = typeof(PropertyInfoModelDataProvider))]
        public void FromPropertyInfo_WithPropertyInfo_InitializesProperties(
            PropertyInfo propertyInfo,
            string propertyName,
            string typeName)
        {
            var model = NavigationPropertyInfo.FromPropertyInfo(propertyInfo);

            Assert.Equal(propertyName, model.PropertyName);
            Assert.Equal(typeName, model.AssemblyQualifiedTypeName);
        }

        [Fact]
        public void FromPropertyInfo_WithNullPropertyInfo_InitializesPropertiesToEmptyStrings()
        {
            var model = NavigationPropertyInfo.FromPropertyInfo(null);

            Assert.Empty(model.PropertyName);
            Assert.Empty(model.AssemblyQualifiedTypeName);
        }

        [Theory]
        [MemberData(
            nameof(PropertyInfoModelDataProvider.InvalidClassNameTestData),
            MemberType = typeof(PropertyInfoModelDataProvider))]
        public void ToPropertyInfo_WithInvalidTypeName_ThrowsCorrectException(
            string propertyName,
            string typeName)
        {
            var model = new NavigationPropertyInfo(propertyName, typeName);

            Assert.Throws<PropertyNotFoundException>(() => model.ToPropertyInfo());
        }

        [Theory]
        [MemberData(
            nameof(PropertyInfoModelDataProvider.InvalidPropertyNameTestData),
            MemberType = typeof(PropertyInfoModelDataProvider))]
        public void ToPropertyInfo_WithInvalidPropertyName_ThrowsCorrectException(
            string propertyName,
            string typeName)
        {
            var model = new NavigationPropertyInfo(propertyName, typeName);

            Assert.Throws<PropertyNotFoundException>(() => model.ToPropertyInfo());
        }

        [Theory]
        [MemberData(
            nameof(PropertyInfoModelDataProvider.ValidPropertyInfoData),
            MemberType = typeof(PropertyInfoModelDataProvider))]
        public void ToPropertyInfo_WithValidData_ReturnsValidProperty(
            PropertyInfo result,
            string propertyName,
            string typeName)
        {
            var model = new NavigationPropertyInfo(propertyName, typeName);

            var propertyInfo = model.ToPropertyInfo();

            Assert.Equal(result, propertyInfo);
        }
    }
}

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
            [CombinatorialValues(null, "", "a", "abc")] string propertyName,
            [CombinatorialValues(null, "", "a", "abc")] string typeName)
        {
            var model = new PropertyInfoModel(propertyName, typeName);

            Assert.NotNull(model.PropertyName);
            Assert.NotNull(model.AssemblyQualifiedTypeName);

            Assert.Equal(propertyName ?? string.Empty, model.PropertyName);
            Assert.Equal(typeName ?? string.Empty, model.AssemblyQualifiedTypeName);
        }

        [Theory]
        [MemberData(
            nameof(PropertyInfoModelDataProvider.ValidPropertyInfoData),
            MemberType = typeof(PropertyInfoModelDataProvider))]
        public void Ctor_WithPropertyInfo_InitializesProperties(
            PropertyInfo propertyInfo,
            string propertyName,
            string typeName)
        {
            var model = new PropertyInfoModel(propertyInfo);

            Assert.NotNull(model.PropertyName);
            Assert.NotNull(model.AssemblyQualifiedTypeName);

            Assert.Equal(propertyName, model.PropertyName);
            Assert.Equal(typeName, model.AssemblyQualifiedTypeName);
        }

        [Fact]
        public void Ctor_WithNullPropertyInfo_InitializesPropertiesToEmptyStrings()
        {
            var model = new PropertyInfoModel(null);

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
            var model = new PropertyInfoModel(propertyName, typeName);

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
            var model = new PropertyInfoModel(propertyName, typeName);

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
            var model = new PropertyInfoModel(propertyName, typeName);

            var propertyInfo = model.ToPropertyInfo();

            Assert.Equal(result, propertyInfo);
        }
    }
}

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

            Assert.Equal(propertyName, model.PropertyName);
            Assert.Equal(typeName, model.AssemblyQualifiedTypeName);
        }

        [Theory]
        [MemberData(
            nameof(PropertyInfoModelDataProvider.PropertyInfoData),
            MemberType = typeof(PropertyInfoModelDataProvider))]
        public void Ctor_WithPropertyInfo_InitializesProperties(
            PropertyInfo propertyInfo,
            string propertyName,
            string typeName)
        {
            var model = new PropertyInfoModel(propertyInfo);

            Assert.Equal(propertyName, model.PropertyName);
            Assert.Equal(typeName, model.AssemblyQualifiedTypeName);
        }

        [Theory]
        [MemberData(
            nameof(PropertyInfoModelDataProvider.PropertyInfoData),
            MemberType = typeof(PropertyInfoModelDataProvider))]
        public void ToProperty_WithValidData_ReturnsValidProperty(
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

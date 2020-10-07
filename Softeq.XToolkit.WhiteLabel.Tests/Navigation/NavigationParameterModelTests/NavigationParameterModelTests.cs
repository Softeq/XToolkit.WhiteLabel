// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Navigation;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Navigation.NavigationParameterModelTests
{
    public class NavigationParameterModelTests
    {
        [Theory]
        [MemberData(
            nameof(NavigationParameterModelDataProvider.CtorData),
            MemberType = typeof(NavigationParameterModelDataProvider))]
        public void Ctor_InitializesProperties(object value, PropertyInfoModel propertyInfo)
        {
            var navigationParameterModel = new NavigationParameterModel(value, propertyInfo);

            Assert.Equal(value, navigationParameterModel.Value);
            Assert.Equal(propertyInfo, navigationParameterModel.PropertyInfo);
        }
    }
}

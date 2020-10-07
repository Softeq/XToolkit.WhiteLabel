// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Navigation;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Navigation.NavigationParameterModelTests
{
    internal static class NavigationParameterModelDataProvider
    {
        private static readonly PropertyInfoModel EmptyPropertyInfoModel = new PropertyInfoModel(null, null);
        private static readonly object EmptyObject = new object();

        public static TheoryData<object, PropertyInfoModel> CtorData
           => new TheoryData<object, PropertyInfoModel>
           {
               { null, null },
               { null, EmptyPropertyInfoModel },
               { EmptyObject, null },
               { EmptyObject, EmptyPropertyInfoModel },
           };
    }
}

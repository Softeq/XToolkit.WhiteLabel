// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Navigation;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Navigation.NavigationParameterModelTests
{
    internal static class NavigationParameterModelDataProvider
    {
        private static readonly NavigationPropertyInfo EmptyPropertyInfoModel = new NavigationPropertyInfo(null, null);
        private static readonly object EmptyObject = new object();

        public static TheoryData<object, NavigationPropertyInfo> CtorData
           => new TheoryData<object, NavigationPropertyInfo>
           {
               { null, null },
               { null, EmptyPropertyInfoModel },
               { EmptyObject, null },
               { EmptyObject, EmptyPropertyInfoModel },
           };
    }
}

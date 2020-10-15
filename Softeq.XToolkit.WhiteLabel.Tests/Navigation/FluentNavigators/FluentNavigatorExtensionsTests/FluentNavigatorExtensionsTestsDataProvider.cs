// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Reflection;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Tests.Stubs;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Navigation.FluentNavigators.FluentNavigatorExtensionsTests
{
    internal static class FluentNavigatorExtensionsTestsDataProvider
    {
        public static readonly string PropertyValue1 = "abc";
        public static readonly int PropertyValue2 = 5;

        private static readonly PropertyInfo ValidPropertyInfo1
            = typeof(ViewModelStub).GetProperty(nameof(ViewModelStub.StringParameter));
        private static readonly PropertyInfoModel ValidPropertyInfoModel1
            = new PropertyInfoModel(ValidPropertyInfo1);

        private static readonly PropertyInfo ValidPropertyInfo2
            = typeof(ViewModelStub).GetProperty(nameof(ViewModelStub.IntParameter));
        private static readonly PropertyInfoModel ValidPropertyInfoModel2
            = new PropertyInfoModel(ValidPropertyInfo2);

        private static readonly NavigationParameterModel ValidNavigationParameter1
            = new NavigationParameterModel(PropertyValue1, ValidPropertyInfoModel1);
        private static readonly NavigationParameterModel ValidNavigationParameter2
            = new NavigationParameterModel(PropertyValue2, ValidPropertyInfoModel2);

        private static readonly NavigationParameterModel InvalidNavigationParameter1
            = null;
        private static readonly NavigationParameterModel InvalidNavigationParameter2
            = new NavigationParameterModel(PropertyValue1, null);
        private static readonly NavigationParameterModel InvalidNavigationParameter3
            = new NavigationParameterModel(null, null);

        public static readonly List<NavigationParameterModel> ValidNavigationParams
            = new List<NavigationParameterModel> { ValidNavigationParameter1, ValidNavigationParameter2 };

        public static TheoryData<IEnumerable<NavigationParameterModel>> InvalidNavigationParams
          => new TheoryData<IEnumerable<NavigationParameterModel>>
          {
               { new List<NavigationParameterModel> { InvalidNavigationParameter1 } },
               { new List<NavigationParameterModel> { InvalidNavigationParameter2 } },
               { new List<NavigationParameterModel> { InvalidNavigationParameter3 } },
               { new List<NavigationParameterModel> { ValidNavigationParameter1, InvalidNavigationParameter1 } },
               { new List<NavigationParameterModel> { ValidNavigationParameter1, InvalidNavigationParameter2 } },
               { new List<NavigationParameterModel> { ValidNavigationParameter1, InvalidNavigationParameter3 } },
               { new List<NavigationParameterModel> { InvalidNavigationParameter1, InvalidNavigationParameter2, InvalidNavigationParameter3 } },
          };
    }
}

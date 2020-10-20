// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Navigation.FluentNavigators
{
    internal static class FluentNavigatorTestsDataProvider
    {
        private static readonly NavigationParameterModel NavigationParam1
            = new NavigationParameterModel(null, null);
        private static readonly NavigationParameterModel NavigationParam2
           = new NavigationParameterModel(null, null);
        private static readonly NavigationParameterModel NavigationParam3
           = new NavigationParameterModel(null, null);

        public static readonly string NavigationParamValue = new string("abc");

        public static TheoryData<IEnumerable<NavigationParameterModel>> NavigationParams
          => new TheoryData<IEnumerable<NavigationParameterModel>>
          {
               { new List<NavigationParameterModel> { null } },
               { new List<NavigationParameterModel> { null, NavigationParam1 } },
               { new List<NavigationParameterModel> { NavigationParam1, NavigationParam1 } },
               { new List<NavigationParameterModel> { NavigationParam1, NavigationParam2 } },
               { new List<NavigationParameterModel> { NavigationParam1, NavigationParam2, NavigationParam3 } },
               { new List<NavigationParameterModel> { NavigationParam1, NavigationParam2, NavigationParam3, NavigationParam2 } },
          };

        public static TheoryData<IEnumerable<NavigationParameterModel>, IEnumerable<NavigationParameterModel>> PairOfNavigationParams
           => new TheoryData<IEnumerable<NavigationParameterModel>, IEnumerable<NavigationParameterModel>>
           {
               {
                   new List<NavigationParameterModel> { null },
                   new List<NavigationParameterModel> { null }
               },
               {
                   new List<NavigationParameterModel> { null },
                   new List<NavigationParameterModel> { null, NavigationParam1 }
               },
               {
                   new List<NavigationParameterModel> { null },
                   new List<NavigationParameterModel> { NavigationParam1, NavigationParam1 }
               },
               {
                   new List<NavigationParameterModel> { null },
                   new List<NavigationParameterModel> { NavigationParam1, NavigationParam2 }
               },
               {
                   new List<NavigationParameterModel> { null },
                   new List<NavigationParameterModel> { null }
               },
               {
                   new List<NavigationParameterModel> { null, NavigationParam1 },
                   new List<NavigationParameterModel> { null }
               },
               {
                   new List<NavigationParameterModel> { NavigationParam1, NavigationParam1 },
                   new List<NavigationParameterModel> { null }
               },
               {
                   new List<NavigationParameterModel> { NavigationParam1, NavigationParam2 },
                   new List<NavigationParameterModel> { null }
               },
               {
                   new List<NavigationParameterModel> { NavigationParam1, NavigationParam2 },
                   new List<NavigationParameterModel> { NavigationParam3 }
               },
               {
                   new List<NavigationParameterModel> { NavigationParam1, NavigationParam2 },
                   new List<NavigationParameterModel> { NavigationParam2 }
               },
               {
                   new List<NavigationParameterModel> { NavigationParam1, NavigationParam2 },
                   new List<NavigationParameterModel> { NavigationParam2, NavigationParam1 }
               },
               {
                   new List<NavigationParameterModel> { NavigationParam1, NavigationParam1 },
                   new List<NavigationParameterModel> { NavigationParam1 }
               },
               {
                   new List<NavigationParameterModel> { NavigationParam1, NavigationParam1 },
                   new List<NavigationParameterModel> { NavigationParam3 }
               },
               {
                   new List<NavigationParameterModel> { NavigationParam1, NavigationParam1 },
                   new List<NavigationParameterModel> { NavigationParam1, NavigationParam1 }
               },
               {
                   new List<NavigationParameterModel> { NavigationParam1, NavigationParam1 },
                   new List<NavigationParameterModel> { NavigationParam3, NavigationParam1 }
               },
               {
                   new List<NavigationParameterModel> { NavigationParam1 },
                   new List<NavigationParameterModel> { NavigationParam3, NavigationParam1 }
               },
               {
                   new List<NavigationParameterModel> { NavigationParam1 },
                   new List<NavigationParameterModel> { NavigationParam1, NavigationParam1 }
               },
               {
                   new List<NavigationParameterModel> { NavigationParam1, NavigationParam2, NavigationParam3 },
                   new List<NavigationParameterModel> { NavigationParam2 }
               },
           };
    }
}

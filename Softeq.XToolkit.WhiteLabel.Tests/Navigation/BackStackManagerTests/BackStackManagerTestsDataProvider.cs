// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Tests.Stubs;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Navigation.BackStackManagerTests
{
    internal static class BackStackManagerTestsDataProvider
    {
        private static readonly IViewModelBase _viewModel1 = new ViewModelStub();
        private static readonly IViewModelBase _viewModel2 = new ViewModelStub();
        private static readonly IViewModelBase _viewModel3 = new ViewModelStub();

        public static TheoryData<List<IViewModelBase>> ClearTestData
           => new TheoryData<List<IViewModelBase>>
           {
               { new List<IViewModelBase>() },
               { new List<IViewModelBase> { null } },
               { new List<IViewModelBase> { _viewModel1 } },
               { new List<IViewModelBase> { _viewModel1, null } },
               { new List<IViewModelBase> { _viewModel1, _viewModel2, null } },
               { new List<IViewModelBase> { null, _viewModel1 } },
               { new List<IViewModelBase> { null, _viewModel1, _viewModel2 } },
               { new List<IViewModelBase> { _viewModel1, null, _viewModel3 } },
               { new List<IViewModelBase> { _viewModel1, _viewModel2, _viewModel3 } },
           };

        public static TheoryData<List<IViewModelBase>, IViewModelBase> PopAndPeekViewModelTestsData
           => new TheoryData<List<IViewModelBase>, IViewModelBase>
           {
               { new List<IViewModelBase> { null }, null },
               { new List<IViewModelBase> { _viewModel1 }, _viewModel1 },
               { new List<IViewModelBase> { _viewModel1, null }, null },
               { new List<IViewModelBase> { _viewModel1, _viewModel2, null }, null },
               { new List<IViewModelBase> { null, _viewModel1 }, _viewModel1 },
               { new List<IViewModelBase> { null, _viewModel1, _viewModel2 }, _viewModel2 },
               { new List<IViewModelBase> { _viewModel1, null, _viewModel3 }, _viewModel3 },
               { new List<IViewModelBase> { _viewModel1, _viewModel2, _viewModel3 }, _viewModel3 },
           };

        public static TheoryData<List<IViewModelBase>, IViewModelBase> PushViewModelTestData
           => new TheoryData<List<IViewModelBase>, IViewModelBase>
           {
               { new List<IViewModelBase>(), null },
               { new List<IViewModelBase>(), _viewModel1 },
               { new List<IViewModelBase> { null }, null },
               { new List<IViewModelBase> { null }, _viewModel1 },
               { new List<IViewModelBase> { _viewModel1 }, null },
               { new List<IViewModelBase> { _viewModel1 }, _viewModel1 },
               { new List<IViewModelBase> { _viewModel2 }, _viewModel1 },
               { new List<IViewModelBase> { _viewModel1, _viewModel2, null }, _viewModel3 },
               { new List<IViewModelBase> { null, _viewModel1 }, _viewModel1 },
               { new List<IViewModelBase> { null, _viewModel1, _viewModel2 }, null },
               { new List<IViewModelBase> { _viewModel1, null, _viewModel3 }, _viewModel2 },
               { new List<IViewModelBase> { _viewModel1, _viewModel2, _viewModel3 }, _viewModel2 },
           };
    }
}

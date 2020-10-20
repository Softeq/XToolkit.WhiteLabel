// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Tests.ViewModels.Tab.ToolbarViewModelBaseTests
{
    internal class TestToolbarViewModelBase : ToolbarViewModelBase<string>
    {
        public void SetTabModels(List<TabItem<string>> tabModels) => TabModels = tabModels;
    }
}

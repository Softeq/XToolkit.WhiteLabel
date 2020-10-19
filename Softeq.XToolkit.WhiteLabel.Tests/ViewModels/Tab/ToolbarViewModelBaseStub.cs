// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Tests.ViewModels.Tab
{
    internal class ToolbarViewModelBaseStub : ToolbarViewModelBase<string>
    {
        public void PublicSetTabModels(List<TabItem<string>> tabModels) => TabModels = tabModels;
    }
}

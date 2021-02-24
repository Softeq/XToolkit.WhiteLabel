// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Tests.Stubs
{
    public class RootFrameNavigationViewModelBaseStub : RootFrameNavigationViewModelBase
    {
        public RootFrameNavigationViewModelBaseStub(IFrameNavigationService frameNavigationService)
            : base(frameNavigationService)
        {
        }

        public override Task NavigateToFirstPageAsync() => throw new NotImplementedException();
    }
}

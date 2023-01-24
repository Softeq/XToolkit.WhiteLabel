// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.Flows;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.CoordinatorPattern
{
    public class FlowRouter : IRouter
    {
        public FlowRouter(IFlowService flowService)
        {
        }

        public void Present<TViewModel>(Action back, bool animated = true) where TViewModel : ViewModelBase
        {
        }

        public void Pop(bool animated = true)
        {
            throw new NotImplementedException();
        }
    }
}

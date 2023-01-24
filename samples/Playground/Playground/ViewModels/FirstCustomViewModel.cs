// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Playground.ViewModels.Flows;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels
{
    public class FirstCustomViewModel : ViewModelBase
    {
        private readonly IFlowService _flowService;

        public FirstCustomViewModel(
            IFlowService flowService)
        {
            OpenNewCustomFlowCommand = new AsyncCommand(OpenNewFlowCommand);
            _flowService = flowService;
        }

        public CustomFlow? CurrentFlow { get; set; }
        public ICommand OpenNewCustomFlowCommand { get; private set; }

        private async Task OpenNewFlowCommand()
        {
            var customFlow = new SecondCustomFlow();
            await _flowService.ProcessAsync(customFlow);
        }
    }
}

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
    public class ThirdCustomViewModel : ViewModelBase
    {
        public ThirdCustomViewModel()
        {
            CloseCurrentFlowCommand = new AsyncCommand(CloseCurrentFlow);
            CloseAllFlowsCommand = new RelayCommand(CloseAllFlows);
        }

        public SecondCustomFlow? CurrentFlow { get; set; }

        public ICommand CloseCurrentFlowCommand { get; private set; }

        public ICommand CloseAllFlowsCommand { get; private set; }

        private void CloseAllFlows()
        {
        }

        private async Task CloseCurrentFlow()
        {
            await CurrentFlow?.Finish();
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Playground.ViewModels.Flows;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.AddEditProfile
{
    public class ProfileDetailsViewModel : ViewModelBase
    {
        private readonly IFlowService _flowService;

        public ProfileDetailsViewModel(
            IFlowService flowService)
        {
            EditCommand = new AsyncCommand(OnEdit);
            _flowService = flowService;
        }

        public string Name { get; set; }

        public ICommand EditCommand { get; private set; }

        public EditProfileFlow CurrentFlow { get; set; }

        private async Task OnEdit()
        {
            var flow = new EditFlow();
            await _flowService.ProcessAsync(flow);
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.TestApproach
{
    public class DetailViewModel : ViewModelBase
    {
        private string _name;
        private readonly ICommonNavigationService _commonNavigationService;

        public DetailViewModel(
            ICommonNavigationService commonNavigationService)
        {
            EditCommand = new AsyncCommand(OnEdit);
            _commonNavigationService = commonNavigationService;
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ICommand EditCommand { get; private set; }

        private async Task OnEdit()
        {
            string result = await _commonNavigationService.NavigateToEditViewModel(Name);
            Name = result;
        }
    }
}

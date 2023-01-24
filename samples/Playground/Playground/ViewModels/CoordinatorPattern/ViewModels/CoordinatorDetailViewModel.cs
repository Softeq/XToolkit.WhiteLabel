// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.CoordinatorPattern.ViewModels
{
    public class CoordinatorDetailViewModel : ViewModelBase
    {
        public CoordinatorDetailViewModel()
        {
            EditCommand = new AsyncCommand(OnEdit);
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ICommand EditCommand { get; private set; }
        public Action<string> Edit { get; internal set; }

        private async Task OnEdit()
        {
            Edit?.Invoke(Name);
            //string result = await _commonNavigationService.NavigateToEditViewModel(Name);
            //Name = result;
        }
    }
}

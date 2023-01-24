// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.CoordinatorPattern.ViewModels
{
    public class CoordinatorEditViewModel : ViewModelBase
    {
        private string _name;

        public CoordinatorEditViewModel()
        {
            SaveCommand = new RelayCommand(OnSave);
        }

        public System.Action<string> OnNext { get; set; }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ICommand SaveCommand { get; private set; }

        private void OnSave()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return;
            }

            OnNext?.Invoke(Name);
        }
    }
}

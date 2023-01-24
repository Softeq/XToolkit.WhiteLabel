// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.TestApproach
{
    public class EditNameViewModel : ViewModelBase
    {
        private string _name;

        public EditNameViewModel()
        {
            SaveCommand = new RelayCommand(OnSave);
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ICommand SaveCommand { get; private set; }

        public TaskCompletionSource<string> CompletionSource { get; set; }

        private void OnSave()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return;
            }

            CompletionSource.TrySetResult(Name);
        }
    }
}

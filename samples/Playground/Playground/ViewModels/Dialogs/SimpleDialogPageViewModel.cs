// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Playground.Models;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.Dialogs
{
    public class SimpleDialogPageViewModel : DialogViewModelBase
    {
        public SimpleDialogPageViewModel()
        {
            CloseCommand = DialogComponent.CloseCommand;
            DoneCommand = new RelayCommand(Done);
        }

        public ICommand CloseCommand { get; }

        public ICommand DoneCommand { get; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        private void Done()
        {
            var person = new Person
            {
                FirstName = FirstName,
                LastName = LastName
            };

            DialogComponent.CloseCommand.Execute(person);
        }
    }
}

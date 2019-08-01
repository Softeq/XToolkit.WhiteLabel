// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Playground.Models;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.Dialogs
{
    public class DialogPageViewModel : DialogViewModelBase
    {
        public DialogPageViewModel()
        {
            CloseCommand = new RelayCommand(Close);
            DoneCommand = new RelayCommand(Done);
        }

        public ICommand CloseCommand { get; }

        public ICommand DoneCommand { get; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        private void Close()
        {
            DialogComponent.CloseCommand.Execute(null);
        }

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

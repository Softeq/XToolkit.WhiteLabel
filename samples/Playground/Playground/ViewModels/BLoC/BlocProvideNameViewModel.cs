// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Windows.Input;
using Playground.ViewModels.BLoC.Flows;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Messenger;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.BLoC
{
    public class BlocProvideNameViewModel : ViewModelBase
    {
        private string _name;

        public BlocProvideNameViewModel()
        {
            SaveCommand = new RelayCommand(OnSave);
            CloseCommand = new RelayCommand(OnClose);
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand CloseCommand { get; set; }

        private void OnSave()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return;
            }

            Messenger.Default.Send<SaveNameMessage>(new SaveNameMessage(Name));
        }

        private void OnClose()
        {
            Messenger.Default.Send<CloseMessage>(new CloseMessage());
        }
    }
}

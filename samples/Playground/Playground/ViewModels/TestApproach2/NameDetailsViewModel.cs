// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Messenger;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.TestApproach2
{
    public class NameDetailsViewModel : ViewModelBase
    {
        private string _name;

        public NameDetailsViewModel()
        {
            Messenger.Default.Register<OnBackMessage>(this, (m) => NavigationDirection.Back.Execute(null));
            EditCommand = new RelayCommand(OnEdit);
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public NavigationDirection NavigationDirection { get; set; }

        public ICommand EditCommand { get; }

        private void OnEdit()
        {
            NavigationDirection.Next.Execute(Name);
        }
    }
}

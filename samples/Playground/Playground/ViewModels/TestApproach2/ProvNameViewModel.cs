using System;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Messenger;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.TestApproach2
{
    public class ProvNameViewModel : ViewModelBase
    {
        private string _name;

        public ProvNameViewModel()
        {
            Messenger.Default.Register<OnBackMessage>(this, (m) => NavigationDirection.Back.Execute(null));
            SaveCommnad = new RelayCommand(OnSave);
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ICommand SaveCommnad { get; private set; }

        public NavigationDirection NavigationDirection { get; set; }

        public void OnSave()
        {
            NavigationDirection.Next.Execute(Name);
        }
    }
}

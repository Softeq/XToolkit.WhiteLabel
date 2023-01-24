// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Windows.Input;
using Playground.ViewModels.RIB.CreateAccount;
using Playground.ViewModels.RIB.Login;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.RIB.Main
{
    public interface IMainPresentableListener
    {
        void DidTapSave(string value);
    }

    public class MainViewModel : ViewModelBase, IMainPresentable
    {
        private string _value;

        public MainViewModel()
        {
            SaveCommand = new RelayCommand(OnSave);
        }

        public string Value
        {
            get => _value;
            set => Set(ref _value, value);
        }

        public IMainListener Lis { get; set; }
        public IMainPresentableListener? Listener { get; set; }

        public ICommand SaveCommand { get; }

        private void OnSave()
        {
            if (string.IsNullOrWhiteSpace(Value))
            {
                return;
            }

            Listener?.DidTapSave(Value);
        }
    }
}

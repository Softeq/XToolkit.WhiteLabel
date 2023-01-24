// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.RIB.CreateAccount
{
    public interface ICreateAccountPresentableListener
    {
        void DidTapClose();
        void DidTapRegister(string name, string password);
    }

    public class CreateAccountViewModel : ViewModelBase, ICreateAccountPresentable
    {
        private string _name;
        private string _password;
        private string _repeatPassword;

        public CreateAccountViewModel()
        {
            RegisterCommand = new RelayCommand(Register);
            CloseCommand = new RelayCommand(Close);
        }

        public ICreateAccountPresentableListener Listener { get; set; }

        public ICommand RegisterCommand { get; }

        public ICommand CloseCommand { get; }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        public string RepeatPassword
        {
            get => _repeatPassword;
            set => Set(ref _repeatPassword, value);
        }

        private void Register()
        {
            if (string.IsNullOrWhiteSpace(Name)
                || string.IsNullOrWhiteSpace(Password)
                || string.IsNullOrWhiteSpace(RepeatPassword)
                || Password != RepeatPassword)
            {
                return;
            }

            Listener?.DidTapRegister(Name, Password);
        }

        private void Close()
        {
            Listener?.DidTapClose();
        }

        public void ShowActivityIndicator(bool isLoading)
        {
            Console.WriteLine(isLoading ? "Loading..." : "Loading stoped");
        }

        public void ShowErrorAlert()
        {
            Console.WriteLine("Alert");
        }
    }
}

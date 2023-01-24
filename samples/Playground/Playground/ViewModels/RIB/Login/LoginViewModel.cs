// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.RIB.Login
{
    public interface ILoginPresentableListener
    {
        void DidTapLogin(string username, string password);
        void DidTapCreateAccount();
    }

    public class LoginViewModel : ViewModelBase, ILoginPresentable
    {
        private string _name;
        private string _password;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
            CreateAccountCommand = new RelayCommand(CreateAccount);
        }

        public ILoginPresentableListener? Listener { get; set; }
        public ICommand LoginCommand { get; }
        public ICommand CreateAccountCommand { get; }

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

        public void ShowActivityIndicator(bool isLoading)
        {
            Console.WriteLine(isLoading ? "Loading..." : "Loading stoped");
        }

        public void ShowErrorAlert()
        {
            Console.WriteLine("Alert");
        }

        private void Login()
        {
            if (string.IsNullOrWhiteSpace(Name)
                || string.IsNullOrWhiteSpace(Password))
            {
                return;
            }

            Listener?.DidTapLogin(Name, Password);
        }

        private void CreateAccount()
        {
            Listener?.DidTapCreateAccount();
        }
    }
}

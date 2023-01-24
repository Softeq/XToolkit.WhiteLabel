// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Windows.Input;
using Playground.ViewModels.RIB.CreateAccount;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.RIB.Detail
{
    public interface IDetailPresentableListener
    {
        void DidTapClose();
        void DidTapSave(string value);
    }

    public class DetailViewModel : ViewModelBase, IDetailPresentable
    {
        private string _value;

        public DetailViewModel()
        {
            EditCommand = new RelayCommand(OnEdit);
            CloseCommand = new RelayCommand(OnClose);
        }

        public string Value
        {
            get => _value;
            set => Set(ref _value, value);
        }

        public IDetailPresentableListener? Listener { get; set; }

        public ICommand EditCommand { get; }
        public ICommand CloseCommand { get; }

        private void OnEdit()
        {
            Listener?.DidTapSave(Value);
        }

        private void OnClose()
        {
            Listener?.DidTapClose();
        }
    }
}

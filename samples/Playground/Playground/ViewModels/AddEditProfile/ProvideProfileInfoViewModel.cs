// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Windows.Input;
using Playground.ViewModels.Flows;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.AddEditProfile
{
    public abstract class ProvideProfileInfoViewModel : ViewModelBase
    {
        private string _name;

        public ProvideProfileInfoViewModel()
        {
            AddCommand = new RelayCommand(Add);
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ICommand AddCommand { get; private set; }

        protected abstract void Add();
    }
}

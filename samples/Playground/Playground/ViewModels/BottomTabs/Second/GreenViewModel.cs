// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.BottomTabs.Second
{
    public class GreenViewModel : ViewModelBase
    {
        private int _count;

        public GreenViewModel()
        {
            IncrementCommand = new RelayCommand(Increment);
        }

        public ICommand IncrementCommand { get; }

        public int Count
        {
            get => _count;
            private set => Set(ref _count, value);
        }

        private void Increment()
        {
            Count++;
        }
    }
}

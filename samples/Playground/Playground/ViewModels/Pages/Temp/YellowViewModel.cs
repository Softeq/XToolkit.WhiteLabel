// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.Pages.Temp
{
    public class YellowViewModel : ViewModelBase
    {
        private int _count;

        public YellowViewModel()
        {
            IncrementCommand = new RelayCommand(Increment);
        }

        public ICommand IncrementCommand { get; }

        public int Count
        {
            get => _count;
            set => Set(ref _count, value);
        }

        private void Increment()
        {
            Count++;
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Pages.Temp
{
    public class GreenViewModel : ViewModelBase
    {
        private readonly IPageNavigationService _pageNavigationService;

        private int _count;

        public GreenViewModel(IPageNavigationService pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;

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
            _pageNavigationService.For<PermissionsPageViewModel>().Navigate();
        }
    }
}

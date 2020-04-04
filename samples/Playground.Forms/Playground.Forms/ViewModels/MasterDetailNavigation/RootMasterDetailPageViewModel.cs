// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.MasterDetailNavigation
{
    public sealed class RootMasterDetailPageViewModel : ViewModelBase, IMasterDetailViewModel
    {
        private readonly MasterPageViewModel _masterPageViewModel;
        private readonly MasterDetailViewModelFactory _viewModelFactory;

        private IViewModelBase _detailViewModel;

        public RootMasterDetailPageViewModel(
            MasterPageViewModel masterPageViewModel,
            IViewModelFactoryService viewModelFactoryService)
        {
            _viewModelFactory = new MasterDetailViewModelFactory(viewModelFactoryService);

            _masterPageViewModel = masterPageViewModel;
            _masterPageViewModel.Initialize(_viewModelFactory.Keys, new RelayCommand<string>(OnMasterItemSelected));

            _detailViewModel = _viewModelFactory.GetViewModelByKey(_masterPageViewModel.Items[0]);
        }

        public IViewModelBase MasterViewModel => _masterPageViewModel;

        public IViewModelBase DetailViewModel
        {
            get => _detailViewModel;
            private set => Set(ref _detailViewModel, value);
        }

        private void OnMasterItemSelected(string item)
        {
            DetailViewModel = _viewModelFactory.GetViewModelByKey(item);
        }
    }
}

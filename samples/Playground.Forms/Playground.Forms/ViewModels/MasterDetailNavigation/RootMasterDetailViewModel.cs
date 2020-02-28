// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.MasterDetailNavigation
{
    public sealed class RootMasterDetailViewModel : ViewModelBase, IMasterDetailViewModel
    {
        private readonly MasterViewModel _masterViewModel;
        private readonly MasterDetailViewModelFactory _viewModelFactory;

        private IViewModelBase _detailViewModel;

        public RootMasterDetailViewModel(
            MasterViewModel masterViewModel,
            IViewModelFactoryService viewModelFactoryService)
        {
            _viewModelFactory = new MasterDetailViewModelFactory(viewModelFactoryService);

            _masterViewModel = masterViewModel;
            _masterViewModel.Initialize(_viewModelFactory.Keys, new RelayCommand<string>(OnMasterItemSelected));

            _detailViewModel = _viewModelFactory.GetViewModelByKey(_masterViewModel.Items[0]);
        }

        public IViewModelBase MasterViewModel => _masterViewModel;

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

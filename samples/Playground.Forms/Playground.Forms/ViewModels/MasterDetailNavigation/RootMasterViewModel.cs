// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Forms.Navigation;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.MasterDetailNavigation
{
    public sealed class RootMasterViewModel : ViewModelBase, IMasterDetailViewModel
    {
        private readonly MasterViewModel _masterViewModel;
        private readonly MasterDetailViewModelFactory _viewModelFactory;

        private IViewModelBase _detailViewModel;

        public RootMasterViewModel(
            MasterViewModel masterViewModel,
            IViewModelFactoryService viewModelFactoryService)
        {
            _masterViewModel = masterViewModel;
            _masterViewModel.ItemSelectedCommand = new RelayCommand<string>(OnMasterItemSelected);

            _viewModelFactory = new MasterDetailViewModelFactory(viewModelFactoryService);
            _detailViewModel = _viewModelFactory.GetViewModelByKey(_masterViewModel.Items[0]);
        }

        public IViewModelBase MasterViewModel => _masterViewModel;

        public IViewModelBase DetailViewModel
        {
            get => _detailViewModel;
            set => Set(ref _detailViewModel, value);
        }

        private void OnMasterItemSelected(string item)
        {
            DetailViewModel = _viewModelFactory.GetViewModelByKey(item);
        }
    }
}

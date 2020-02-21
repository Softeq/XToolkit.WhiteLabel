// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Forms.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.Forms.ViewModels.MasterDetailNavigation
{
    public class RootMasterViewModel : ViewModelBase, IMasterDetailViewModel
    {
        private readonly MasterViewModel _masterViewModel;
        private ViewModelBase _detailViewModel;
        private readonly IContainer _container;

        public RootMasterViewModel(
            MasterViewModel masterViewModel,
            DetailViewModel detailViewModel,
            IContainer container)
        {
            _masterViewModel = masterViewModel;
            _masterViewModel.ItemSelectedCommand = new RelayCommand<string>(OnMasterItemSelected);

            _detailViewModel = detailViewModel;
            _container = container;
        }

        public ViewModelBase MasterViewModel => _masterViewModel;

        public ViewModelBase DetailViewModel
        {
            get => _detailViewModel;
            set => Set(ref _detailViewModel, value);
        }

        private void OnMasterItemSelected(string item)
        {
            if (item == "root")
            {
                var detail = _container.Resolve<DetailViewModel>();
                DetailViewModel = detail;
                return;
            }

            var viewModel = _container.Resolve<SelectedItemViewModel>();
            viewModel.Title = item;
            DetailViewModel = viewModel;
        }
    }
}

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.Dialogs
{
    public class SelectPresentationStyleViewModel : DialogViewModelBase
    {
        public SelectPresentationStyleViewModel()
        {
            StyleIds = new ObservableRangeCollection<string>
            {
                "DickPresentationStyle",
                "FormSheetPresentationStyle"
            };

            SelectItemCommand = new RelayCommand<string>(SelectItem);
        }

        public ObservableRangeCollection<string> StyleIds { get; }

        public ICommand<string> SelectItemCommand { get; }

        private void SelectItem(string viewModel)
        {
            DialogComponent.CloseCommand.Execute(viewModel);
        }
    }
}

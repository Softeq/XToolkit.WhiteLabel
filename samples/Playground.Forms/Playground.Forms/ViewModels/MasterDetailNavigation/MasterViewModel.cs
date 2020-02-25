// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.Forms.ViewModels.MasterDetailNavigation
{
    public class MasterViewModel : ViewModelBase
    {
        public ObservableCollection<string> Items { get; private set; } = new ObservableCollection<string>();

        public RelayCommand<string>? ItemSelectedCommand { get; private set; }

        public void Initialize(IReadOnlyList<string> keys, RelayCommand<string> itemSelectedCommand)
        {
            Items = new ObservableCollection<string>(keys);
            ItemSelectedCommand = itemSelectedCommand;
        }
    }
}

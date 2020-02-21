// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.ObjectModel;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.Forms.ViewModels.MasterDetailNavigation
{
    public class MasterViewModel : ViewModelBase
    {
        public ObservableCollection<string> Items { get; } = new ObservableCollection<string>
        {
            "page 1",
            "page 2",
            "page 3",
            "page 4",
            "root"
        };

        public RelayCommand<string>? ItemSelectedCommand { get; set; }
    }
}

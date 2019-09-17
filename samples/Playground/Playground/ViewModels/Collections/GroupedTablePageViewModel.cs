// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Services;
using Playground.ViewModels.Collections.Base;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Collections
{
    public class GroupedTablePageViewModel : GroupedListViewModelBase
    {
        public GroupedTablePageViewModel(IDialogsService dialogsService, IDataService dataService) : base(dialogsService, dataService)
        {
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Extended;
using Playground.Services;
using Playground.ViewModels.Collections.Base;

namespace Playground.ViewModels.Collections
{
    public class GroupedTablePageViewModel : GroupedListViewModelBase
    {
        public GroupedTablePageViewModel(IExtendedDialogsService dialogsService, IDataService dataService)
            : base(dialogsService, dataService)
        {
        }
    }
}

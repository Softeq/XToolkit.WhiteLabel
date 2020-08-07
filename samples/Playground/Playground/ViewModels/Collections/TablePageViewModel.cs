// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Extended;
using Playground.Services;
using Playground.ViewModels.Collections.Base;

namespace Playground.ViewModels.Collections
{
    public class TablePageViewModel : SimpleListViewModelBase
    {
        public TablePageViewModel(
            IDataService dataService,
            IExtendedDialogsService dialogsService)
            : base(dataService, dialogsService)
        {
        }
    }
}

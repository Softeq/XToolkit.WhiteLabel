// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Extended;
using Playground.Services;
using Playground.ViewModels.Collections.Base;

namespace Playground.ViewModels.Collections
{
    public class GroupedCollectionPageViewModel : GroupedListViewModelBase
    {
        public GroupedCollectionPageViewModel(IExtendedDialogsService dialogsService, IDataService dataService)
            : base(dialogsService, dataService)
        {
        }
    }
}

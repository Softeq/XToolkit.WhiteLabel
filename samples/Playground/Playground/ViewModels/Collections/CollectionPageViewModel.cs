// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Extended;
using Playground.Services;
using Playground.ViewModels.Collections.Base;

namespace Playground.ViewModels.Collections
{
    public class CollectionPageViewModel : SimpleListViewModelBase
    {
        public CollectionPageViewModel(
            IDataService dataService,
            IExtendedDialogsService dialogsService)
            : base(dataService, dialogsService)
        {
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Services;
using Playground.ViewModels.Collections.Base;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Collections
{
    public class GroupedCollectionPageViewModel : GroupedListViewModelBase
    {
        public GroupedCollectionPageViewModel(IDialogsService dialogsService, IDataService dataService) : base(dialogsService, dataService)
        {
        }
    }
}

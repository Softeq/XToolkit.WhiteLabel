// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using Playground.Models;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.Collections
{
    public class GroupedCollectionPageViewModel : ViewModelBase
    {
        public GroupedCollectionPageViewModel()
        {
            ItemModels.AddRange(GenerateItems());
        }

        public ObservableRangeCollection<ItemViewModel> ItemModels { get; } = new ObservableRangeCollection<ItemViewModel>();

        private IEnumerable<ItemViewModel> GenerateItems()
        {
            return Enumerable.Range(0, 1000).Select(i => new ItemViewModel
            {
                Title = $"{i} #- Title",
                IconUrl = $"https://picsum.photos/100/150?random={i}"
            });
        }
    }
}

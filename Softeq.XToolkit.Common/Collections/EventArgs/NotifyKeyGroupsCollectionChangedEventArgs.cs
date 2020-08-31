// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Collections.Specialized;

namespace Softeq.XToolkit.Common.Collections.EventArgs
{
    public class NotifyKeyGroupsCollectionChangedEventArgs : System.EventArgs
    {
        public NotifyKeyGroupsCollectionChangedEventArgs(NotifyCollectionChangedAction action, List<int> oldSectionsSizes)
        {
            Action = action;
            OldSectionsSizes = oldSectionsSizes ?? new List<int>();
            OldSectionsCount = OldSectionsSizes.Count;
        }

        public NotifyCollectionChangedAction Action { get; }
        public List<int> ModifiedSectionsIndexes { get; set; } = new List<int>();
        public IList<(int Section, IList<int> ModifiedIndexes)> ModifiedItemsIndexes { get; } = new List<(int, IList<int>)>();
        public int OldSectionsCount { get; }
        public IList<int> OldSectionsSizes { get; }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Softeq.XToolkit.Common.Collections
{
    public class ObservableItemContentRangeCollection<T> : ObservableRangeCollection<T> where T : INotifyPropertyChanged
    {
        public ObservableItemContentRangeCollection()
        {
            CollectionChanged += ObservableItemContentRangeCollectionChanged;
        }

        public ObservableItemContentRangeCollection(IEnumerable<T> items) : this()
        {
            AddRange(items);
        }

        private void ObservableItemContentRangeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    ((INotifyPropertyChanged) item).PropertyChanged += ItemPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    ((INotifyPropertyChanged) item).PropertyChanged -= ItemPropertyChanged;
                }
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, sender, sender,
                IndexOf((T) sender));
            OnCollectionChanged(args);
        }
    }
}

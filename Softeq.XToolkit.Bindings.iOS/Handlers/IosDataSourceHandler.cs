// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Collections;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Handlers
{
    public class IosDataSourceHandler
    {
        public static void Handle<TKey, TItem>(
            UICollectionView collectionView,
            NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> args)
        {
            var handler = new IosCollectionObservableKeyGroupCollectionHandler<TKey, TItem>(collectionView);

            handler.Handle(args);
        }

        public static void Handle<TKey, TItem>(
            UITableView tableView,
            NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> args)
        {
            var handler = new IosTableObservableKeyGroupCollectionHandler<TKey, TItem>(tableView);

            handler.Handle(args);
        }
    }
}

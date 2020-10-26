// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.Model;

namespace Softeq.XToolkit.WhiteLabel.ViewModels
{
    /// <summary>
    ///     A base view model for working with paginated data.
    /// </summary>
    /// <typeparam name="TViewModel">The type of view model.</typeparam>
    /// <typeparam name="TModel">The type of data model.</typeparam>
    public abstract class PaginationViewModelBase<TViewModel, TModel> : ObservableObject
    {
        private const int DefaultPageSize = 20;

        private bool _canLoadMore;
        private int _currentPage = -1;

        protected PaginationViewModelBase()
        {
            PageSize = DefaultPageSize;
            CancellationToken = CancellationToken.None;
            Items = new ObservableRangeCollection<TViewModel>();
            LoadMoreCommand = new AsyncCommand(LoadMoreAsync, () => CanLoadMore);
        }

        /// <summary>
        ///     Gets or sets the index of the currently loaded page.
        /// </summary>
        public int CurrentPage
        {
            get => _currentPage;
            protected set => Set(ref _currentPage, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether additional data can be loaded.
        /// </summary>
        public bool CanLoadMore
        {
            get => _canLoadMore || CanAlwaysLoadMore;
            protected set
            {
                if (Set(ref _canLoadMore, value))
                {
                    ((AsyncCommand) LoadMoreCommand).RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        ///     Gets a command to load more data.
        /// </summary>
        public IAsyncCommand LoadMoreCommand { get; }

        /// <summary>
        ///     Gets the list of all loaded items.
        /// </summary>
        public ObservableRangeCollection<TViewModel> Items { get; }

        /// <summary>
        ///     Gets a value indicating whether additional data should be forced to be loaded.
        /// </summary>
        protected virtual bool CanAlwaysLoadMore { get; }

        /// <summary>
        ///     Gets the size of the page that will be used to load data.
        /// </summary>
        protected virtual int PageSize { get; }

        /// <summary>
        ///     Gets the cancellation token that will be used to cancel the requests.
        /// </summary>
        protected virtual CancellationToken CancellationToken { get; }

        /// <summary>
        ///     Resets the list of all loaded items.
        /// </summary>
        public void ResetItems()
        {
            Interlocked.Exchange(ref _currentPage, -1);

            Items.Clear();
        }

        /// <summary>
        ///     Loads the first page of data.
        ///     This method must be called first to initialize the view model.
        /// </summary>
        /// <param name="cancellationToken">Token for canceling the request.</param>
        /// <returns>Task.</returns>
        protected async Task LoadFirstPageAsync(CancellationToken cancellationToken)
        {
            Interlocked.Exchange(ref _currentPage, 0);

            var viewModels = await LoadPageAsync(_currentPage).ConfigureAwait(false);

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            Execute.BeginOnUIThread(() =>
            {
                Items.ReplaceRange(viewModels);
                ItemsDidLoad(viewModels.Any());
            });
        }

        private async Task LoadNextPageAsync(CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _currentPage);

            var viewModels = await LoadPageAsync(_currentPage).ConfigureAwait(false);

            if (cancellationToken.IsCancellationRequested)
            {
                Interlocked.Decrement(ref _currentPage);
                return;
            }

            var hasResults = viewModels.Any();

            if (hasResults)
            {
                Items.AddRange(viewModels);
            }
            else
            {
                Execute.BeginOnUIThread(() =>
                {
                    CanLoadMore = false;
                });

                Interlocked.Decrement(ref _currentPage);
            }

            Execute.BeginOnUIThread(() =>
            {
                ItemsDidLoad(hasResults);
            });
        }

        private async Task LoadMoreAsync()
        {
            if (Items.Count < PageSize)
            {
                return;
            }

            await LoadNextPageAsync(CancellationToken).ConfigureAwait(false);
        }

        private async Task<IList<TViewModel>> LoadPageAsync(int pageNumber)
        {
            Execute.BeginOnUIThread(ItemsWillLoad);

            var pagingModel = await LoadAsync(pageNumber, PageSize).ConfigureAwait(false);
            if (pagingModel == null)
            {
                return new List<TViewModel>();
            }

            if (CanAlwaysLoadMore && pagingModel.Data.Count == 0)
            {
                return new List<TViewModel>();
            }

            Execute.BeginOnUIThread(() =>
            {
                CanLoadMore = pagingModel.Page + 1 < pagingModel.TotalNumberOfPages;
            });

            var viewModels = MapItemsToViewModels(pagingModel.Data);

            FilterItems(viewModels);

            return viewModels;
        }

        protected abstract IList<TViewModel> MapItemsToViewModels(IList<TModel> models);

        protected virtual IList<TViewModel> FilterItems(IList<TViewModel> viewModels)
        {
            return viewModels;
        }

        protected abstract Task<PagingModel<TModel>?> LoadAsync(int pageIndex, int pageSize);

        protected virtual void ItemsWillLoad()
        {
        }

        protected virtual void ItemsDidLoad(bool hasResults)
        {
        }
    }
}

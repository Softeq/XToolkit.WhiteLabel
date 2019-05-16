// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Models;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.ViewModels
{
    public abstract class PaginationViewModelBase<TViewModel, TModel> : ObservableObject
    {
        private const int DefaultPageSize = 20;

        private int _currentPage = -1;
        private bool _canLoadMore;

        protected PaginationViewModelBase()
        {
            LoadMoreCommand = new RelayCommand(LoadMore, () => CanLoadMore);
        }

        public int CurrentPage
        {
            get => _currentPage;
            protected set => Set(ref _currentPage, value);
        }

        public bool CanLoadMore
        {
            get => _canLoadMore || CanAlwaysLoadMore;
            protected set => Set(ref _canLoadMore, value);
        }

        public ICommand LoadMoreCommand { get; }

        public ObservableRangeCollection<TViewModel> Items { get; } = new ObservableRangeCollection<TViewModel>();

        public void ResetItems()
        {
            Interlocked.Exchange(ref _currentPage, 0);

            Items.Clear();
        }

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

        private void LoadMore()
        {
            if (Items.Count < PageSize)
            {
                return;
            }
            Task.Run(() => LoadNextPageAsync(CancellationToken));
        }

        private async Task<IList<TViewModel>> LoadPageAsync(int pageNumber)
        {
            Execute.BeginOnUIThread(() =>
            {
                ItemsWillLoad();
            });

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
                CanLoadMore = pagingModel.Page < pagingModel.TotalNumberOfPages;
            });

            var viewModels = MapItemsToViewModels(pagingModel.Data);

            FilterItems(viewModels);

            return viewModels;
        }

        protected virtual bool CanAlwaysLoadMore { get; } = false;

        protected virtual int PageSize { get; } = DefaultPageSize;

        protected virtual CancellationToken CancellationToken { get; } = CancellationToken.None;

        protected abstract IList<TViewModel> MapItemsToViewModels(IList<TModel> models);

        protected virtual IList<TViewModel> FilterItems(IList<TViewModel> viewModels)
        {
            return viewModels;
        }

        protected virtual Task<PagingModel<TModel>> LoadAsync(int pageNumber, int pageSize)
        {
            return Task.FromResult(default(PagingModel<TModel>));
        }

        protected virtual void ItemsWillLoad()
        {
        }

        protected virtual void ItemsDidLoad(bool hasResults)
        {
        }
    }
}

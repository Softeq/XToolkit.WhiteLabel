// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Model;

namespace Softeq.XToolkit.WhiteLabel.ViewModels
{
    public abstract class PaginationSearchViewModelBase<TViewModel, TModel>
        : PaginationViewModelBase<TViewModel, TModel>
    {
        private bool _hasContent;
        private bool _isBusy;
        private CancellationTokenSource _lastSearchCancelSource = new CancellationTokenSource();
        private string _searchQuery = string.Empty;

        protected PaginationSearchViewModelBase()
        {
            ClearCommand = new RelayCommand(DoClear);
            SearchCommand = new RelayCommand(DoSearch);
        }

        public ICommand ClearCommand { get; }

        public ICommand SearchCommand { get; }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery == value)
                {
                    return;
                }

                _searchQuery = value?.Trim();
                RaisePropertyChanged();

                if (string.IsNullOrEmpty(_searchQuery) && !AllowEmptySearchQuery)
                {
                    EmptyQuery();
                }
                else
                {
                    SearchCommand.Execute(null);
                }
            }
        }

        public bool HasResults
        {
            get => _hasContent;
            set => Set(ref _hasContent, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        protected virtual int SearchDelay => 300;

        protected virtual bool SilentLoadPagesEnabled => true;

        protected virtual bool AllowEmptySearchQuery => false;

        protected override CancellationToken CancellationToken => _lastSearchCancelSource.Token;

        protected abstract Task<PagingModel<TModel>> LoadAsync(string query, int pageNumber, int pageSize);

        protected override async Task<PagingModel<TModel>> LoadAsync(int pageNumber, int pageSize)
        {
            try
            {
                return await LoadAsync(SearchQuery, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return default;
        }

        protected override void ItemsWillLoad()
        {
            ShowProgress(true);
        }

        protected override void ItemsDidLoad(bool hasResults)
        {
            ShowProgress(false);
            HasResults = hasResults || CurrentPage > 0;
        }

        protected virtual void EmptyQuery()
        {
            ResetItems();
            HasResults = false;

            _lastSearchCancelSource?.Cancel();
            ShowProgress(false);
        }

        protected virtual void DoClear()
        {
            SearchQuery = string.Empty;
        }

        protected virtual void ShowProgress(bool isBusy)
        {
            if (CurrentPage > 0 && SilentLoadPagesEnabled)
            {
                IsBusy = false;
                return;
            }

            IsBusy = isBusy;
        }

        protected virtual async void DoSearch()
        {
            // reset to initial state
            ResetItems();
            HasResults = true;

            await InternalSearchAsync().ConfigureAwait(false);
        }

        protected virtual async Task InternalSearchAsync()
        {
            try
            {
                Interlocked.Exchange(ref _lastSearchCancelSource, new CancellationTokenSource()).Cancel();

                await Task.Delay(SearchDelay, _lastSearchCancelSource.Token).ConfigureAwait(false);

                await LoadFirstPageAsync(_lastSearchCancelSource.Token).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // ignored
            }
        }

        protected virtual void HandleException(Exception ex)
        {
        }
    }
}

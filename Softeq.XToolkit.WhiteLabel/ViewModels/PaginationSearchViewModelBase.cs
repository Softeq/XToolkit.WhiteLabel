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
    /// <summary>
    ///    A base view model for searching and representing results as paginated data.
    /// </summary>
    /// <typeparam name="TViewModel">The type of view model.</typeparam>
    /// <typeparam name="TModel">The type of data model.</typeparam>
    public abstract class PaginationSearchViewModelBase<TViewModel, TModel>
        : PaginationViewModelBase<TViewModel, TModel>
    {
        private bool _hasContent;
        private bool _isBusy;
        private CancellationTokenSource _lastSearchCancelSource = new CancellationTokenSource();
        private string? _searchQuery = string.Empty;

        protected PaginationSearchViewModelBase()
        {
            ClearCommand = new RelayCommand(DoClear);
            SearchCommand = new RelayCommand(DoSearch);
        }

        /// <summary>
        ///     Gets the command to clear <see cref="SearchQuery"/> property.
        /// </summary>
        public ICommand ClearCommand { get; }

        /// <summary>
        ///     Gets the command to perform the search.
        /// </summary>
        public ICommand SearchCommand { get; }

        /// <summary>
        ///     Gets or sets the search query.
        /// </summary>
        /// <remarks>
        ///     Empty values will be ignored by default, to change this behavior
        ///     you can override <see cref="AllowEmptySearchQuery"/> property.
        /// </remarks>
        public string? SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery == value)
                {
                    return;
                }

                _searchQuery = value;
                RaisePropertyChanged();

                if (string.IsNullOrWhiteSpace(_searchQuery) && !AllowEmptySearchQuery)
                {
                    EmptyQuery();
                }
                else
                {
                    SearchCommand.Execute(null);
                }
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether any result was found.
        /// </summary>
        public bool HasResults
        {
            get => _hasContent;
            protected set => Set(ref _hasContent, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this ViewModel is doing some work right now.
        /// </summary>
        /// <remarks>
        ///     By default during the loading of more data, this value will not be used.
        ///     You can override <see cref="SilentLoadPagesEnabled"/> property to change this behavior.
        /// </remarks>
        public bool IsBusy
        {
            get => _isBusy;
            protected set => Set(ref _isBusy, value);
        }

        /// <summary>
        ///     Gets the delay value between searches.
        /// </summary>
        protected virtual int SearchDelay => 300;

        /// <summary>
        ///     Gets a value indicating whether property <see cref="IsBusy"/> will be changed
        ///     when additional pages are loaded.
        /// </summary>
        protected virtual bool SilentLoadPagesEnabled => true;

        /// <summary>
        ///     Gets a value indicating whether empty search query validation is disabled.
        /// </summary>
        protected virtual bool AllowEmptySearchQuery => false;

        protected override CancellationToken CancellationToken => _lastSearchCancelSource.Token;

        protected abstract Task<PagingModel<TModel>> LoadAsync(string? query, int pageNumber, int pageSize);

        protected override async Task<PagingModel<TModel>?> LoadAsync(int pageNumber, int pageSize)
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

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.Model;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public abstract class ViewModelWithPagingBase<T> : ViewModelBase
    {
        private const int PageSizeConst = 15;

        private bool _canLoadMoreRecords;
        private int _currentPage = -1;
        private int _locker;

        public int CurrentPage
        {
            get => _currentPage;
            protected set
            {
                _currentPage = value;
                RaisePropertyChanged();
            }
        }

        public bool CanLoadMoreRecords
        {
            get => _canLoadMoreRecords || CanAlwaysLoadMoreRecords;
            set => Set(ref _canLoadMoreRecords, value);
        }

        protected virtual bool CanAlwaysLoadMoreRecords => false;

        protected virtual int PageSize => PageSizeConst;

        protected virtual async Task LoadNextPageAsync(bool shouldReset, CancellationToken cancellationToken)
        {
            if (Interlocked.Exchange(ref _locker, 1) == 1)
            {
                return;
            }

            var model = await GetItems(CurrentPage + 1, PageSize).ConfigureAwait(false);

            if (cancellationToken.IsCancellationRequested)
            {
                Interlocked.Exchange(ref _locker, 0);
                return;
            }

            if (model?.Data != null)
            {
                CurrentPage = model.Page;
                await AddPage(model.Data, shouldReset).ConfigureAwait(false);
                Execute.BeginOnUIThread(() => { CanLoadMoreRecords = model.Page < model.TotalNumberOfPages; });
            }
            else if (model == null && CurrentPage == 0)
            {
                CurrentPage = -1;
                await AddPage(default(List<T>)!, shouldReset).ConfigureAwait(false);
                Execute.BeginOnUIThread(() => { CanLoadMoreRecords = false; });
            }

            Interlocked.Exchange(ref _locker, 0);
        }

        protected virtual Task LoadFirstPageAsync(CancellationToken cancellationToken)
        {
            CurrentPage = 0;
            return LoadNextPageAsync(true, cancellationToken);
        }

        protected abstract Task<PagingModel<T>> GetItems(int page, int perPage);

        protected abstract Task<bool> AddPage(IList<T> data, bool shouldReset);
    }
}
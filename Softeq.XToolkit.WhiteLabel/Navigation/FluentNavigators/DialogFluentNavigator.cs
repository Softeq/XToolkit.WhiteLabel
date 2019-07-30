// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Extensions;

namespace Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators
{
    public class DialogFluentNavigator<TViewModel> : FluentNavigatorBase<TViewModel>
        where TViewModel : IDialogViewModel
    {
        private readonly IDialogsService _dialogsService;

        public DialogFluentNavigator(IDialogsService dialogsService)
        {
            _dialogsService = dialogsService;
        }

        public Task<IDialogResult<TResult>> NavigateWithResult<TResult>()
        {
            return _dialogsService.ShowForViewModelAsync<TViewModel, TResult>(Parameters);
        }

        public Task<IDialogResult> NavigateWithResult()
        {
            return _dialogsService.ShowForViewModelAsync<TViewModel>(Parameters);
        }

        public Task<TResult> Navigate<TResult>()
        {
            return _dialogsService
                .ShowForViewModelAsync<TViewModel, TResult>(Parameters)
                .ReturnWhenDissmissed();
        }

        public Task Navigate()
        {
            return _dialogsService
                .ShowForViewModelAsync<TViewModel>(Parameters)
                .WaitUntilDissmissed();
        }

        public DialogFluentNavigator<TViewModel> WithParam<TValue>(
            Expression<Func<TViewModel, TValue>> property,
            TValue value)
        {
            ApplyParameter(property, value);
            return this;
        }
    }
}

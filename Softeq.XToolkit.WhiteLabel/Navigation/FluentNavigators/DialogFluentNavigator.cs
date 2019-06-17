// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

        public Task<TResult> Navigate<TResult>()
        {
            return _dialogsService.ShowForViewModel<TViewModel, TResult>(Parameters);
        }

        public Task Navigate()
        {
            return _dialogsService.ShowForViewModel<TViewModel>(Parameters);
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

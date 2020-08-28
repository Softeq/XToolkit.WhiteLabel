// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators
{
    public class DialogFluentNavigator<TViewModel> : FluentNavigatorBase<TViewModel>
        where TViewModel : DialogViewModelBase
    {
        private readonly IDialogsService _dialogsService;

        private bool _awaitResult;

        public DialogFluentNavigator(IDialogsService dialogsService)
        {
            _dialogsService = dialogsService;
        }

        /// <summary>
        ///     Set value to the target ViewModel property.
        /// </summary>
        /// <typeparam name="TValue">Type of parameter value.</typeparam>
        /// <param name="propertyExpression">Target property.</param>
        /// <param name="value">Value for set.</param>
        /// <returns></returns>
        public DialogFluentNavigator<TViewModel> WithParam<TValue>(
            Expression<Func<TViewModel, TValue>> propertyExpression,
            TValue value)
        {
            ApplyParameter(propertyExpression, value);
            return this;
        }

        /// <summary>
        ///     Use when you need to get the result faster than dialog will be dismissed.
        ///     Avoid using a queue of dialogs in this case.
        /// </summary>
        /// <returns></returns>
        public DialogFluentNavigator<TViewModel> WithAwaitResult()
        {
            _awaitResult = true;
            return this;
        }

        /// <summary>
        ///     Open dialog with awaiting until dialog will be dismissed.
        /// </summary>
        /// <returns></returns>
        public Task Navigate()
        {
            return _dialogsService
                .ShowForViewModelAsync<TViewModel>(Parameters)
                .WaitUntilDismissed();
        }

        /// <summary>
        ///     Open dialog with awaiting result until dialog will be dismissed.
        ///     For awaiting result use method <see cref="WithAwaitResult"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of dialog result.</typeparam>
        /// <returns></returns>
        public async Task<TResult> Navigate<TResult>()
        {
            var showDialogTask = _dialogsService.ShowForViewModelAsync<TViewModel, TResult>(Parameters);

            if (_awaitResult)
            {
                var result = await showDialogTask;
                return result.Value;
            }

            return await showDialogTask.WaitUntilDismissed();
        }
    }
}

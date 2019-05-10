// Developed for PAWS-HALO by Softeq Development
// Corporation http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.Navigation.NavigationHelpers
{
    public class DialogNavigationHelper<TViewModel> : NavigationHelper<TViewModel>
        where TViewModel : IDialogViewModel
    {
        private readonly IDialogsService _dialogsService;

        public DialogNavigationHelper(IDialogsService dialogsService)
        {
            _dialogsService = dialogsService;
        }

        public Task<TResult> Navigate<TResult>()
        {
            return _dialogsService.ShowForViewModel<TViewModel, TResult>(Parameters);
        }

        public Task Navigate()
        {
            return _dialogsService.ShowForViewModel<TViewModel>();
        }

        public new DialogNavigationHelper<TViewModel> WithParam<TValue>(Expression<Func<TViewModel, TValue>> property,
            TValue value)
        {
            base.WithParam(property, value);
            return this;
        }
    }
}
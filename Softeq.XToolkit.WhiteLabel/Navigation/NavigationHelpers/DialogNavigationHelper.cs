// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.Navigation.NavigationHelpers
{
    public class DialogNavigationHelper<TViewModel> : NavigationHelper<TViewModel>
        where TViewModel : IDialogViewModel
    {
        private readonly IDialogsService _storyboardDialogsService;

        public Task<TResult> Navigate<TResult>()
        {
            return _storyboardDialogsService.ShowForViewModel<TViewModel, TResult>(Parameters);
        }

        public Task Navigate()
        {
            return _storyboardDialogsService.ShowForViewModel<TViewModel>();
        }

        public DialogNavigationHelper(IDialogsService storyboardDialogsService)
        {
            _storyboardDialogsService = storyboardDialogsService;
        }

        public new DialogNavigationHelper<TViewModel> WithParam<TValue>(Expression<Func<TViewModel, TValue>> property,
            TValue value)
        {
            base.WithParam(property, value);
            return this;
        }
    }
}

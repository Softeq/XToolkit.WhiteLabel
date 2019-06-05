// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public class BackStackManager : IBackStackManager
    {
        private readonly IIocContainer _iocContainer;
        private readonly Stack<IViewModelBase> _backStack;

        public BackStackManager(IIocContainer iocContainer)
        {
            _iocContainer = iocContainer;
            _backStack = new Stack<IViewModelBase>();
        }

        public int Count => _backStack.Count;

        public void PushViewModel(IViewModelBase viewModel)
        {
            _backStack.Push(viewModel);
        }

        public IViewModelBase PopViewModel()
        {
            return _backStack.Pop();
        }

        public void Clear()
        {
            _backStack.Clear();
        }

        public TViewModel GetExistingOrCreateViewModel<TViewModel>()
            where TViewModel : IViewModelBase
        {
            IViewModelBase viewModel;

            if (_backStack.Count > 0)
            {
                viewModel = _backStack.Peek();

                if (viewModel is TViewModel viewModelBase)
                {
                    return viewModelBase;
                }

                throw new ArgumentException(
                    $"Couldn't find ViewModel for type: {typeof(TViewModel)}. " +
                    $"Please use {nameof(PageNavigationService)} navigating, " +
                    $"instead of navigation via StartActivity().");
            }

            // Used to recreate ViewModel if processes or activity was killed
            viewModel = _iocContainer.Resolve<TViewModel>();

            _backStack.Push(viewModel);

            return (TViewModel) viewModel;
        }
    }
}
// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class BackStackManager : IBackStackManager
    {
        private readonly Stack<IViewModelBase> _backStack;

        public BackStackManager()
        {
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

        public TViewModel GetExistingOrCreateViewModel<TViewModel>() where TViewModel : IViewModelBase
        {
            if (_backStack.TryPeek(out var viewModel))
            {
                if (viewModel.GetType() != typeof(TViewModel))
                {
                    throw new ArgumentException($"Please use {nameof(PageNavigationService)} navigating, instead d of navigation via StartActivity()");
                }

                return (TViewModel)viewModel;
            }

            //Used to recreate viewmodel if processes or activity was killed
            viewModel = ServiceLocator.Resolve<TViewModel>();
            _backStack.Push(viewModel);

            return (TViewModel)viewModel;
        }
    }
}
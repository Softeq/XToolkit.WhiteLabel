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

        public void Clear()
        {
            _backStack.Clear();
        }

        public IViewModelBase PopViewModel()
        {
            return _backStack.Pop();
        }

        public IViewModelBase GetExistingOrCreateViewModel(Type type)
        {
            if (_backStack.TryPeek(out var viewModel))
            {
                return viewModel;
            }

            //Used to recreate viewmodel if processes or activity was killed
            viewModel = (IViewModelBase) ServiceLocator.Resolve(type);
            _backStack.Push(viewModel);

            return viewModel;
        }
    }
}
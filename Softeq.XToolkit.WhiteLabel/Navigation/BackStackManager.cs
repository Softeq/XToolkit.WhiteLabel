// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public class BackStackManager : IBackStackManager
    {
        private readonly Stack<IViewModelBase> _backStack;
        private readonly IContainer _iocContainer;

        public BackStackManager(IContainer iocContainer)
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
            }

            // used to create ViewModel when the page was created by system
            viewModel = _iocContainer.Resolve<TViewModel>();

            _backStack.Push(viewModel);

            return (TViewModel) viewModel;
        }
    }
}

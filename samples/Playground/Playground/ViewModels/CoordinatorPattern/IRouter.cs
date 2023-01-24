// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.CoordinatorPattern
{
    public interface IRouter
    {
        void Present<TViewModel>(Action back, bool animated = true)
            where TViewModel : ViewModelBase;

        void Pop(bool animated = true);
    }
}

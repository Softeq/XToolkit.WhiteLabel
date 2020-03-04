// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public interface IViewLocator
    {
        [Obsolete("Never used inside components. Please use full signature: `GetTargetType(Type, ViewType)`")]
        Type GetTargetType<TViewModel>(ViewType viewType);

        Type GetTargetType(Type viewModelType, ViewType viewType);

        object GetView(IViewModelBase viewModel, ViewType viewType);
    }
}

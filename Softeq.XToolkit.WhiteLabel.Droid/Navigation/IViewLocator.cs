// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public interface IViewLocator
    {
        void Initialize(Dictionary<Type, Type> viewModelToView);

        Type GetTargetType<T>(ViewType viewType);

        Type GetTargetType(Type type, ViewType viewType);

        object GetView(IViewModelBase viewModel, ViewType viewType);

        // TODO YP: rework approach for inject FrameNavifationService, without reflection
        // currently, used only on Android
        [Obsolete("Don't use this method.")]
        void TryInjectParameters(object viewModel, object parameter, string parameterName);
    }
}

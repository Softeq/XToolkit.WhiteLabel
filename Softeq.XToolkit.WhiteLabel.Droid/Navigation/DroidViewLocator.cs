// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class DroidViewLocator : IViewLocator
    {
        private readonly ViewModelToViewMap _viewModelToViewMap;

        public DroidViewLocator(ViewModelToViewMap viewModelToViewMap)
        {
            _viewModelToViewMap = viewModelToViewMap;
        }

        public Type GetTargetType<TViewModel>(ViewType viewType)
        {
            return GetTargetType(typeof(TViewModel), viewType);
        }

        // TODO YP: export to base, looks the same as iOS impl.
        public virtual Type GetTargetType(Type viewModelType, ViewType viewType)
        {
            if (_viewModelToViewMap.TryGetValue(viewModelType, out var targetViewType))
            {
                return targetViewType;
            }

            var targetViewTypeName = BuildViewTypeName(viewModelType, viewType);
            targetViewType = Type.GetType(targetViewTypeName) ?? AssemblySource.FindTypeByNames(new[] { targetViewTypeName });

            if (targetViewType == null)
            {
                throw new InvalidOperationException($"Can't find target view type: {targetViewTypeName}");
            }

            return targetViewType;
        }

        public virtual object GetView(IViewModelBase viewModel, ViewType viewType)
        {
            var targetType = GetTargetType(viewModel.GetType(), viewType);
            var view = Activator.CreateInstance(targetType);

            if (view is IBindable bindable)
            {
                bindable.SetDataContext(viewModel);
            }

            return view;
        }

        protected virtual string BuildViewTypeName(Type viewModelType, ViewType viewType)
        {
            if (viewModelType == null)
            {
                throw new ArgumentNullException(nameof(viewModelType));
            }

            return viewModelType.FullName?
                .Replace(".ViewModels.", ".Droid.Views.")
                .Replace("ViewModel", viewType.ToString());
        }
    }
}

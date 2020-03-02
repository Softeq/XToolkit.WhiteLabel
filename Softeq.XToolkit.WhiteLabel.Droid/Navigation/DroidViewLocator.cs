// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class DroidViewLocator : IViewLocator
    {
        private Dictionary<Type, Type> _modelToViewTypes;

        public DroidViewLocator()
        {
            _modelToViewTypes = new Dictionary<Type, Type>();
        }

        public void Initialize(Dictionary<Type, Type> viewModelToView)
        {
            if (viewModelToView == null)
            {
                return;
            }

            _modelToViewTypes = viewModelToView;
        }

        public Type GetTargetType<TViewModel>(ViewType viewType)
        {
            return GetTargetType(typeof(TViewModel), viewType);
        }

        public Type GetTargetType(Type viewModelType, ViewType viewType)
        {
            if (_modelToViewTypes.TryGetValue(viewModelType, out var typeOfView))
            {
                return typeOfView;
            }

            var typeName = viewModelType.FullName;
            return GetTargetType(typeName, viewType);
        }

        public object GetView(IViewModelBase viewModel, ViewType viewType)
        {
            var targetType = GetTargetType(viewModel.GetType(), viewType);
            var view = Activator.CreateInstance(targetType);

            if (view is IBindable bindable)
            {
                bindable.SetDataContext(viewModel);
            }

            return view;
        }

        private Type GetTargetType(string viewModelTypeName, ViewType viewType)
        {
            var targetTypeName = viewModelTypeName.Replace(".ViewModels.", ".Droid.Views.");
            targetTypeName = targetTypeName.Replace("ViewModel", viewType.ToString());

            var targetType = Type.GetType(targetTypeName) ?? AssemblySource.FindTypeByNames(new[] { targetTypeName });

            if (targetType == null)
            {
                throw new InvalidOperationException($"Can't find target type: {targetTypeName}");
            }

            return targetType;
        }
    }
}

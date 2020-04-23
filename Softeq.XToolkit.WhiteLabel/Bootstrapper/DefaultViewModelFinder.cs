// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Softeq.XToolkit.WhiteLabel.Extensions;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper
{
    public interface IViewModelFinder
    {
        ViewModelToViewMap GetViewModelToViewMapping(IEnumerable<Assembly> assemblies);

        bool IsViewType(Type type);
    }

    public class DefaultViewModelFinder : IViewModelFinder
    {
        private readonly Type[] _viewsTypes;

        public DefaultViewModelFinder(params Type[] viewsTypes)
        {
            _viewsTypes = viewsTypes;
        }

        public virtual ViewModelToViewMap GetViewModelToViewMapping(IEnumerable<Assembly> assemblies)
        {
            var viewModelToViewMap = new ViewModelToViewMap();

            foreach (var viewType in assemblies.SelectMany(SelectViewsTypes))
            {
                var viewModelType = viewType.BaseType.GetGenericArguments()[0];

                if (!viewModelType.IsAbstract)
                {
                    viewModelToViewMap.Add(viewModelType, viewType);
                }
            }

            return viewModelToViewMap;
        }

        public virtual bool IsViewType(Type type)
        {
            return _viewsTypes.Any(t => t.IsAssignableFrom(type));
        }

        protected virtual IEnumerable<Type> SelectViewsTypes(Assembly assembly)
        {
            return assembly.GetTypes().View(_viewsTypes);
        }
    }

    public class ViewModelToViewMap : Dictionary<Type, Type>
    {
    }
}

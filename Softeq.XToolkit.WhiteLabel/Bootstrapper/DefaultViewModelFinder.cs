// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Softeq.XToolkit.WhiteLabel.Extensions;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper
{
    /// <summary>
    ///     An interface that declares logic about lookup ViewModel types from known assemblies.
    /// </summary>
    public interface IViewModelFinder
    {
        ViewModelToViewMap GetViewModelToViewMapping(IEnumerable<Assembly> assemblies);

        bool IsViewType(Type type);
    }

    /// <summary>
    ///     The default implementation of <see cref="IViewModelFinder"/>.
    /// </summary>
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

    /// <summary>
    ///     The type alias to ViewModel to View mapping.
    /// </summary>
    public class ViewModelToViewMap : Dictionary<Type, Type>
    {
    }
}

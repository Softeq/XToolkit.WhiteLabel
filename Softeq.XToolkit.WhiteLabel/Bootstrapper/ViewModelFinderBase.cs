// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper
{
    public abstract class ViewModelFinderBase
    {
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

        public abstract bool IsViewType(Type type);

        protected abstract IEnumerable<Type> SelectViewsTypes(Assembly assembly);
    }

    public class ViewModelToViewMap : Dictionary<Type, Type>
    {
    }
}

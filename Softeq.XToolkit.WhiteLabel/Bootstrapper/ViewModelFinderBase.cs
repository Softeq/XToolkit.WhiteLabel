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
        public virtual Dictionary<Type, Type> GetViewModelToViewMapping(IEnumerable<Assembly> assemblies)
        {
            var viewModelToViewDictionary = new Dictionary<Type, Type>();

            foreach (var type in assemblies.SelectMany(SelectViewModelTypes))
            {
                var viewModelType = type.BaseType.GetGenericArguments()[0];

                if (!viewModelType.IsAbstract)
                {
                    viewModelToViewDictionary.Add(viewModelType, type);
                }
            }

            return viewModelToViewDictionary;
        }

        protected abstract IEnumerable<Type> SelectViewModelTypes(Assembly assembly);
    }
}

using System;
using System.Collections.Generic;
using DryIoc;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;

namespace Softeq.XToolkit.WhiteLabel.iOS
{
    public abstract class AutoRegistrationAppDelegate : AppDelegateBase
    {
        protected override void StartScopeForIoc()
        {
            var container = new Container(rules => rules.WithoutFastExpressionCompiler());
            ConfigureIoc(container);
            RegisterInternalServices(container);

            var viewModelToViewControllerDictionary = new Dictionary<Type, Type>();

            viewModelToViewControllerDictionary = CreateAndRegisterMissedViewModels(container);

            Dependencies.IocContainer.StartScope(container);

            Dependencies.IocContainer.Resolve<IViewLocator>().Initialize(viewModelToViewControllerDictionary);
        }

        private Dictionary<Type, Type> CreateAndRegisterMissedViewModels(Container builder)
        {
            var viewModelToViewControllerTypes = new Dictionary<Type, Type>();

            foreach (var type in GetType().Assembly.GetTypes().View("ViewController"))
            {
                var viewModelType = type.BaseType.GetGenericArguments()[0];
                viewModelToViewControllerTypes.Add(viewModelType, type);

                builder.TryPerDependency(viewModelType);
            }

            return viewModelToViewControllerTypes;
        }
    }
}
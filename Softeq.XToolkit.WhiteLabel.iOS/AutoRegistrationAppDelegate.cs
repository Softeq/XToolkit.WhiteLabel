using System;
using System.Collections.Generic;
using Autofac;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;

namespace Softeq.XToolkit.WhiteLabel.iOS
{
    public abstract class AutoRegistrationAppDelegate : AppDelegateBase
    {
        protected override void StartScopeForIoc()
        {
            var containerBuilder = new ContainerBuilder();
            ConfigureIoc(containerBuilder);
            RegisterInternalServices(containerBuilder);

            var viewModelToViewControllerDictionary = new Dictionary<Type, Type>();

            viewModelToViewControllerDictionary = CreateAndRegisterMissedViewModels(containerBuilder);

            Dependencies.IocContainer.StartScope(containerBuilder);

            Dependencies.IocContainer.Resolve<IViewLocator>().Initialize(viewModelToViewControllerDictionary);
        }

        private Dictionary<Type, Type> CreateAndRegisterMissedViewModels(ContainerBuilder builder)
        {
            var viewModelToViewControllerTypes = new Dictionary<Type, Type>();

            foreach (var type in GetType().Assembly.GetTypes().View("ViewController"))
            {
                var viewModelType = type.BaseType.GetGenericArguments()[0];
                viewModelToViewControllerTypes.Add(viewModelType, type);

                builder.RegisterType(viewModelType).PreserveExistingDefaults();
            }

            return viewModelToViewControllerTypes;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using UIKit;

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

            foreach (var type in SelectAssemblies().SelectMany(x => x.GetTypes().View(typeof(UIViewController))))
            {
                var viewModelType = type.BaseType.GetGenericArguments()[0];
                viewModelToViewControllerTypes.Add(viewModelType, type);

                builder.PerDependency(viewModelType).PreserveExistingDefaults();
            }

            return viewModelToViewControllerTypes;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.Runtime;
using Autofac;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Extensions;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    public abstract class AutoRegistrationMainApplication : MainApplicationBase
    {
        protected AutoRegistrationMainApplication(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
        }

        protected override void StartScopeForIoc()
        {
            var containerBuilder = new ContainerBuilder();
            ConfigureIoc(containerBuilder);
            RegisterInternalServices(containerBuilder);

            var viewModelToViewControllerDictionary = new Dictionary<Type, Type>();

            viewModelToViewControllerDictionary = CreateAndRegisterMissedViewModels(containerBuilder);

            Dependencies.IocContainer.StartScope(containerBuilder);

            Dependencies.IocContainer.Resolve<ViewLocator>().Initialize(viewModelToViewControllerDictionary);
        }

        private Dictionary<Type, Type> CreateAndRegisterMissedViewModels(ContainerBuilder builder)
        {
            var viewModelToViewControllerTypes = new Dictionary<Type, Type>();

            foreach (var type in GetType().Assembly.GetTypes()
                .View(((ViewType[])Enum.GetValues(typeof(ViewType))).Select(x => x.ToString()).ToArray()))
            {
                var viewModelType = type.BaseType.GetGenericArguments()[0];
                viewModelToViewControllerTypes.Add(viewModelType, type);

                builder.PerDependency(viewModelType).PreserveExistingDefaults();
            }

            return viewModelToViewControllerTypes;
        }
    }
}

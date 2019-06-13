// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Plugin.CurrentActivity;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.Tab;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;
using IContainer = Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract.IContainer;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    public class DroidBootstrapper : BootstrapperBase
    {
        protected override IContainer BuildContainer(IContainerBuilder builder, IList<Assembly> assemblies)
        {
            var viewModelToViewControllerDictionary = CreateAndRegisterMissedViewModels(builder, assemblies);

            builder.Singleton<IViewLocator>(x =>
            {
                var viewLocator = x.Resolve<ViewLocator>();
                viewLocator.Initialize(viewModelToViewControllerDictionary);
                return viewLocator;
            }).PreserveExistingDefaults();

            return base.BuildContainer(builder, assemblies);
        }

        protected override void RegisterInternalServices(IContainerBuilder builder)
        {
            builder.Singleton(c => CrossCurrentActivity.Current)
                .PreserveExistingDefaults();
            builder.Singleton<ActivityPageNavigationService, IPlatformNavigationService>()
                .PreserveExistingDefaults();
            builder.PerDependency<FrameNavigationService, IFrameNavigationService>()
                .PreserveExistingDefaults();
            builder.Singleton<ViewLocator>()
                .PreserveExistingDefaults();
            builder.PerDependency<RootFrameNavigationViewModel>()
                .PreserveExistingDefaults();
            builder.Singleton<TabNavigationService, ITabNavigationService>()
                .PreserveExistingDefaults();
        }

        protected Dictionary<Type, Type> CreateAndRegisterMissedViewModels(IContainerBuilder builder, IList<Assembly> assemblies)
        {
            var viewModelToViewTypes = new Dictionary<Type, Type>();

            foreach (var type in assemblies.SelectMany(assembly => assembly.GetTypes()
                .View(typeof(AppCompatActivity), typeof(Fragment), typeof(DialogFragment))))
            {
                var viewModelType = type.BaseType.GetGenericArguments()[0];
                viewModelToViewTypes.Add(viewModelType, type);

                builder.PerDependency(viewModelType).PreserveExistingDefaults();
            }

            return viewModelToViewTypes;
        }

        protected override void ConfigureIoc(IContainerBuilder builder)
        {
        }
    }
}

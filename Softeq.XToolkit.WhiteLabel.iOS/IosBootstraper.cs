// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS.Interfaces;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.iOS.Services;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.Tab;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS
{
    public class IosBootstrapper : BootstrapperBase
    {
        protected override IContainer BuildContainer(IContainerBuilder builder, IList<Assembly> assemblies)
        {
            var viewModelToViewControllerDictionary = CreateAndRegisterMissedViewModels(builder, assemblies);

            builder.Singleton<IViewLocator>(x =>
            {
                var viewLocator = x.Resolve<StoryboardViewLocator>();
                viewLocator.Initialize(viewModelToViewControllerDictionary);
                return viewLocator;
            }, IfRegistered.Keep);

            return base.BuildContainer(builder, assemblies);
        }

        protected override void RegisterInternalServices(IContainerBuilder builder)
        {
            builder.Singleton<ViewControllerProvider, IViewControllerProvider>(IfRegistered.Keep);
            builder.Singleton<StoryboardViewLocator>(IfRegistered.Keep);
            builder.Singleton<StoryboardNavigation, IPlatformNavigationService>(IfRegistered.Keep);
            builder.PerDependency<StoryboardFrameNavigationService, IFrameNavigationService>(IfRegistered.Keep);
            builder.PerDependency<TabViewModel>(IfRegistered.Keep);
            builder.Singleton<TabNavigationService, ITabNavigationService>(IfRegistered.Keep);
        }

        private static Dictionary<Type, Type> CreateAndRegisterMissedViewModels(IContainerBuilder builder,
            IEnumerable<Assembly> assemblies)
        {
            var viewModelToViewControllerTypes = new Dictionary<Type, Type>();

            foreach (var type in assemblies.SelectMany(x => x.GetTypes().View(typeof(UIViewController))))
            {
                try
                {
                    var viewModelType = type.BaseType.GetGenericArguments()[0];
                    viewModelToViewControllerTypes.Add(viewModelType, type);

                    builder.PerDependency(viewModelType, IfRegistered.Keep);
                }
                catch (Exception ex)
                {
                }
            }

            return viewModelToViewControllerTypes;
        }

        protected override void ConfigureIoc(IContainerBuilder builder)
        {
        }
    }
}

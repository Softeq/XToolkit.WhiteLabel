// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
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
        protected override void BuildContainer(ContainerBuilder builder, IList<Assembly> assemblies)
        {
            var viewModelToViewControllerDictionary = CreateAndRegisterMissedViewModels(builder, assemblies);

            builder.Singleton<IViewLocator>(x =>
            {
                var viewLocator = x.Resolve<StoryboardViewLocator>();
                viewLocator.Initialize(viewModelToViewControllerDictionary);
                return viewLocator;
            }).PreserveExistingDefaults();

            base.BuildContainer(builder, assemblies);
        }

        protected override void RegisterInternalServices(ContainerBuilder builder)
        {
            builder.Singleton<ViewControllerProvider, IViewControllerProvider>()
                .PreserveExistingDefaults();
            builder.Singleton<StoryboardViewLocator>()
                .PreserveExistingDefaults();
            builder.Singleton<StoryboardNavigation, IPlatformNavigationService>()
                .PreserveExistingDefaults();
            builder.PerDependency<StoryboardFrameNavigationService, IFrameNavigationService>()
                .PreserveExistingDefaults();
            builder.PerDependency<RootFrameNavigationViewModel>()
                .PreserveExistingDefaults();
            builder.Singleton<TabNavigationService, ITabNavigationService>()
                .PreserveExistingDefaults();
        }

        protected Dictionary<Type, Type> CreateAndRegisterMissedViewModels(ContainerBuilder builder, IList<Assembly> assemblies)
        {
            var viewModelToViewControllerTypes = new Dictionary<Type, Type>();

            foreach (var type in assemblies.SelectMany(x => x.GetTypes().View(typeof(UIViewController))))
            {
                var viewModelType = type.BaseType.GetGenericArguments()[0];
                viewModelToViewControllerTypes.Add(viewModelType, type);

                builder.PerDependency(viewModelType).PreserveExistingDefaults();
            }

            return viewModelToViewControllerTypes;
        }

        protected override void ConfigureIoc(ContainerBuilder builder)
        {
        }
    }
}

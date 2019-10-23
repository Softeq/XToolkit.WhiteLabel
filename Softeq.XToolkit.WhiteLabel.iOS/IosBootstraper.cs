// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS.Dialogs;
using Softeq.XToolkit.WhiteLabel.iOS.Interfaces;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.iOS.Services;
using Softeq.XToolkit.WhiteLabel.Navigation;
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

            var stylesDictionary = CreateAndRegisterPresentationStyles(assemblies);

            builder.Singleton<IosPresentationStyleStorage>(x =>
            {
                var storage = new IosPresentationStyleStorage();
                storage.Initialize(stylesDictionary);
                return storage;
            }, IfRegistered.Keep);

            return base.BuildContainer(builder, assemblies);
        }

        protected override void RegisterInternalServices(IContainerBuilder builder)
        {
            builder.Singleton<ViewControllerProvider, IViewControllerProvider>(IfRegistered.Keep);
            builder.Singleton<StoryboardViewLocator>(IfRegistered.Keep);
            builder.Singleton<StoryboardNavigation, IPlatformNavigationService>(IfRegistered.Keep);
            builder.PerDependency<StoryboardFrameNavigationService, IFrameNavigationService>(IfRegistered.Keep);
        }

        private static Dictionary<Type, Type> CreateAndRegisterMissedViewModels(IContainerBuilder builder,
            IEnumerable<Assembly> assemblies)
        {
            var viewModelToViewControllerTypes = new Dictionary<Type, Type>();

            foreach (var type in assemblies.SelectMany(x => x.GetTypes().View(typeof(UIViewController))))
            {
                var viewModelType = type.BaseType.GetGenericArguments()[0];

                viewModelToViewControllerTypes.Add(viewModelType, type);

                builder.PerDependency(viewModelType, IfRegistered.Keep);
            }

            return viewModelToViewControllerTypes;
        }

        private static Dictionary<string, Type> CreateAndRegisterPresentationStyles(IEnumerable<Assembly> assemblies)
        {
            var types = new Dictionary<string, Type>();

            foreach (var type in assemblies.SelectMany(x => x.GetTypes())
                .Where(x => x.IsSubclassOf(typeof(PresentationArgsBase))))
            {
                var attribute = type.GetCustomAttribute<PresentationStyleAttribute>();

                types.Add(attribute.Id, type);
            }

            return types;
        }

        protected override void ConfigureIoc(IContainerBuilder builder)
        {
        }
    }
}

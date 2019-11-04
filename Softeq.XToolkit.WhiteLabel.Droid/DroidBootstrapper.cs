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

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    public class DroidBootstrapper : BootstrapperBase
    {
        protected override IContainer BuildContainer(IContainerBuilder builder, IList<Assembly> assemblies)
        {
            var viewModelToViewControllerDictionary = CreateAndRegisterMissedViewModels(builder, assemblies);

            builder.Singleton<IViewLocator>(container =>
            {
                var viewLocator = container.Resolve<DroidViewLocator>();
                viewLocator.Initialize(viewModelToViewControllerDictionary);
                return viewLocator;
            }, IfRegistered.Keep);

            return base.BuildContainer(builder, assemblies);
        }

        protected override void RegisterInternalServices(IContainerBuilder builder)
        {
            base.RegisterInternalServices(builder);

            // common
            builder.Singleton(c => CrossCurrentActivity.Current, IfRegistered.Keep);

            // navigation
            builder.Singleton<ActivityPageNavigationService, IPlatformNavigationService>(IfRegistered.Keep);
            builder.Singleton<BundleService, IBundleService>(IfRegistered.Keep);
            builder.Singleton<DroidViewLocator>(IfRegistered.Keep);
            builder.PerDependency<DroidFrameNavigationService, IFrameNavigationService>(IfRegistered.Keep);
        }

        private static Dictionary<Type, Type> CreateAndRegisterMissedViewModels(
            IContainerBuilder builder,
            IEnumerable<Assembly> assemblies)
        {
            var viewModelToViewTypes = new Dictionary<Type, Type>();
            var targetTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes()
                    .View(typeof(AppCompatActivity), typeof(Fragment), typeof(DialogFragment)));

            foreach (var type in targetTypes)
            {
                var viewModelType = type.BaseType.GetGenericArguments()[0];
                viewModelToViewTypes.Add(viewModelType, type);

                builder.PerDependency(viewModelType, IfRegistered.Keep);
            }

            return viewModelToViewTypes;
        }

        protected override void ConfigureIoc(IContainerBuilder builder)
        {
        }
    }
}

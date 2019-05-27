// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
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

            var viewModelToViewDictionary = new Dictionary<Type, Type>();

            viewModelToViewDictionary = CreateAndRegisterMissedViewModels(containerBuilder);

            Dependencies.IocContainer.StartScope(containerBuilder);

            Dependencies.IocContainer.Resolve<IViewLocator>().Initialize(viewModelToViewDictionary);
        }

        private Dictionary<Type, Type> CreateAndRegisterMissedViewModels(ContainerBuilder builder)
        {
            var viewModelToViewTypes = new Dictionary<Type, Type>();

            foreach (var type in SelectAssemblies().SelectMany(assembly => assembly.GetTypes()
                .View(typeof(AppCompatActivity), typeof(Fragment), typeof(DialogFragment))))
            {
                var viewModelType = type.BaseType.GetGenericArguments()[0];
                viewModelToViewTypes.Add(viewModelType, type);

                builder.PerDependency(viewModelType).PreserveExistingDefaults();
            }

            return viewModelToViewTypes;
        }
    }
}

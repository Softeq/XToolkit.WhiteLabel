// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Threading;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms
{
    public abstract class FormsApp : Application
    {
        private IBootstrapper? _bootstrapper;
        private Func<List<Assembly>>? _getAssembliesFunc;

        protected FormsApp(IBootstrapper bootstrapper, Func<List<Assembly>> getAssembliesFunc)
        {
            _bootstrapper = bootstrapper;
            _getAssembliesFunc = getAssembliesFunc;

            // Init UI thread helper
            PlatformProvider.Current = new FormsPlatformProvider();
        }

        protected override void OnStart()
        {
            base.OnStart();

            InitializeBootstrapper();
        }

        /// <summary>
        ///     Application developers override this method to perform actions when the application start was completed
        /// </summary>
        protected virtual void OnStarted()
        {
        }

        /// <summary>
        ///     The predicate of extracting type for storing in the cache
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual bool IsMatchedToExtract(Type type)
        {
            return typeof(Page).IsAssignableFrom(type);
        }

        private void InitializeBootstrapper()
        {
            Task.Run(() =>
            {
                if (_bootstrapper != null && _getAssembliesFunc != null)
                {
                    var assemblies = _getAssembliesFunc();

                    InitAssemblySource(assemblies);
                    _bootstrapper.Init(assemblies);

                    _bootstrapper = null;
                    _getAssembliesFunc = null;

                    Execute.BeginOnUIThread(OnStarted);
                }
            }).FireAndForget();
        }

        private void InitAssemblySource(List<Assembly> assemblies)
        {
            AssemblySourceCache.Install();
            AssemblySourceCache.ExtractTypes = assembly => assembly.GetExportedTypes().Where(IsMatchedToExtract);
            AssemblySource.Instance.ReplaceRange(assemblies);
        }
    }
}

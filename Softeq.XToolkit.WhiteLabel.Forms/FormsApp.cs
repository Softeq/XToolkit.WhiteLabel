// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
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
        }

        protected override void OnStart()
        {
            base.OnStart();

            Task.Run(() =>
            {
                if (_bootstrapper != null && _getAssembliesFunc != null)
                {
                    _bootstrapper.Init(_getAssembliesFunc());
                    _bootstrapper = null;
                    _getAssembliesFunc = null;
                    Execute.BeginOnUIThread(OnStarted);
                }
            }).FireAndForget();
        }

        protected virtual void OnStarted()
        {
        }
    }
}

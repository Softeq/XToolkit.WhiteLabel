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

        protected FormsApp(IBootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper;

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

        private void InitializeBootstrapper()
        {
            Task.Run(() =>
            {
                if (_bootstrapper != null)
                {
                    _bootstrapper.Initialize();
                    _bootstrapper = null;

                    Execute.BeginOnUIThread(OnStarted);
                }
            }).FireAndForget();
        }
    }
}

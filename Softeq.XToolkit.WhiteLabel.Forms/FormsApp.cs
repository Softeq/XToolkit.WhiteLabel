// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms
{
    /// <summary>
    ///     Based on <see cref="T:Xamarin.Forms.Application"/> class that represents a cross-platform mobile application.
    /// </summary>
    public abstract class FormsApp : Application
    {
        private IBootstrapper? _bootstrapper;

        protected FormsApp(IBootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper;

            // Init UI thread helper
            Execute.Initialize(new FormsMainThreadExecutor());
        }

        protected override void OnStart()
        {
            base.OnStart();

            InitializeBootstrapper();
        }

        /// <summary>
        ///     Application developers override this method to perform actions when the application start was completed.
        /// </summary>
        /// <param name="container">
        ///     IoC container, configured by bootstrapper.
        /// </param>
        protected virtual void OnStarted(IContainer container)
        {
        }

        private void InitializeBootstrapper()
        {
            Task.Run(() =>
            {
                if (_bootstrapper != null)
                {
                    var container = _bootstrapper.Initialize();
                    _bootstrapper = null;

                    Dependencies.Initialize(container);
                    Execute.BeginOnUIThread(() => OnStarted(container));
                }
            }).FireAndForget(Console.WriteLine);
        }
    }
}

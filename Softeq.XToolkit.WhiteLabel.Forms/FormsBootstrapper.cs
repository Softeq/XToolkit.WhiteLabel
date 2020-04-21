// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Forms.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Forms
{
    /// <inheritdoc />
    public abstract class FormsBootstrapper : BootstrapperBase
    {
        /// <inheritdoc />
        protected override void RegisterInternalServices(IContainerBuilder builder)
        {
            base.RegisterInternalServices(builder);

            builder.PerDependency<FormsFrameNavigationService, IFrameNavigationService>(IfRegistered.Replace);
            builder.PerDependency<FormsNavigationService, IPlatformNavigationService>(IfRegistered.Replace);

            builder.Singleton<FormsViewLocator, IFormsViewLocator>(IfRegistered.Replace);
            builder.Singleton<FormsPageNavigationService, IPageNavigationService>(IfRegistered.Replace);
            builder.Singleton<FormsDialogsService, IDialogsService>(IfRegistered.Replace);
        }
    }
}

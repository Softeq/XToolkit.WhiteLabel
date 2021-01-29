// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    /// <summary>
    ///     ViewModel that corresponds to the root page in the FrameNavigation stack.
    /// </summary>
    public abstract class RootFrameNavigationViewModelBase : ViewModelBase
    {
        protected RootFrameNavigationViewModelBase(IFrameNavigationService frameNavigationService)
        {
            FrameNavigationService = frameNavigationService
                ?? throw new ArgumentNullException(nameof(frameNavigationService));
        }

        /// <summary>
        ///     Gets the <see cref="IFrameNavigationService"/> implementation that will be used for navigation.
        /// </summary>
        public IFrameNavigationService FrameNavigationService { get; }

        /// <summary>
        ///     Gets a value indicating whether this ViewModel is initialized.
        /// </summary>
        public new bool IsInitialized => FrameNavigationService.IsInitialized;

        /// <summary>
        ///     Initializes <see cref="FrameNavigationService"/> instance.
        /// </summary>
        /// <param name="navigation">
        ///     Navigation object (usually platform-specific) that is used for initialization.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="navigation"/> cannot be <see langword="null"/>.
        /// </exception>
        public void InitializeNavigation(object navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            FrameNavigationService.Initialize(navigation);
        }

        /// <summary>
        ///     Creates the root page and navigates to it.
        /// </summary>
        public abstract Task NavigateToFirstPageAsync();

        /// <summary>
        ///     Restores the last fragment after a switch between tabs or back navigation.
        /// </summary>
        public virtual void RestoreState()
        {
            // Check fast-backward nav
            if (FrameNavigationService.CanGoBack)
            {
                FrameNavigationService.RestoreNavigation();
            }
            else
            {
                NavigateToFirstPageAsync();
            }
        }
    }
}
// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public class RootFrameNavigationPageViewModel<T> : RootFrameNavigationViewModelBase where T : IViewModelBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RootFrameNavigationPageViewModel{T}"/> class.
        /// </summary>
        /// <param name="frameNavigationService">
        ///     The <see cref="IFrameNavigationService"/> implementation that will be used for navigation.
        /// </param>
        public RootFrameNavigationPageViewModel(IFrameNavigationService frameNavigationService)
            : base(frameNavigationService)
        {
        }

        /// <inheritdoc/>
        public override void NavigateToFirstPage()
        {
            FrameNavigationService.NavigateToViewModel<T>(true, null);
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.Frames
{
    public class SplitFrameViewModel : ViewModelBase
    {
        public SplitFrameViewModel()
        {
            TopShellViewModel = Dependencies.Container.Resolve<TopShellViewModel>();
        }

        public TopShellViewModel TopShellViewModel { get; }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface ITabViewModel : IViewModelBase
    {
        public string Title { get; }
        public string IconImageSource { get; }
    }
}

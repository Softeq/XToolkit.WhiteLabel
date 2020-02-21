// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Forms.Navigation
{
    public interface IMasterDetailViewModel
    {
        ViewModelBase MasterViewModel { get; }
        ViewModelBase DetailViewModel { get; }
    }
}

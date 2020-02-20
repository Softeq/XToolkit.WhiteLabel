// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Navigation
{
    public interface IViewLocator
    {
        Page GetPage(object viewModel);
    }
}

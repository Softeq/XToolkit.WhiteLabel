// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Navigation
{
    public interface IFormsViewLocator
    {
        Page GetPage(object viewModel);
    }
}

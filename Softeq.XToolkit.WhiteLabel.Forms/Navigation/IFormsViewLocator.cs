// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Navigation
{
    public interface IFormsViewLocator
    {
        Task<Page> GetPageAsync(object viewModel);
        INavigation? FindNavigationForViewModel(INavigation navigation, object viewModel);
    }
}

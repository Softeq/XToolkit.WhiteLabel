// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Model
{
    public class TabbedItem
    {
        public TabbedItem(string title, IViewModelBase viewModel, string sourceImage)
        {
            TabTitle = title;
            ViewModel = viewModel;
            TabSourceImage = sourceImage;
        }

        public string TabTitle { get; }
        public string TabSourceImage { get; }
        public IViewModelBase ViewModel { get; }
    }
}

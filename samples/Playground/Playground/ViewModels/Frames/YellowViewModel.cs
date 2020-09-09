// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Messenger;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.Frames
{
    public class YellowViewModel : ViewModelBase
    {
        public YellowViewModel()
        {
            BackCommand = new RelayCommand(GoBack);
        }

        public RelayCommand BackCommand { get; }

        public string BackText { get; } = "prev frame";

        private void GoBack()
        {
            Messenger.Default.Send(new GoBackMessage());
        }
    }
}

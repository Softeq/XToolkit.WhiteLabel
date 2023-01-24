// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Playground.ViewModels.Flows;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels
{
    public class SecondCustomViewModel : ViewModelBase
    {
        public SecondCustomViewModel()
        {
            OpenNextScreenCommand = new RelayCommand(OpenNextScreen);
        }

        public SecondCustomFlow? CurrentFlow { get; set; }

        public ICommand OpenNextScreenCommand { get; private set; }

        public void OpenNextScreen()
        {
            CurrentFlow.NavigateToSecondStep();
        }
    }
}

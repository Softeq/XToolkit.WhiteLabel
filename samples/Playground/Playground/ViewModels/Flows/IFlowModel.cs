// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Flows
{
    public interface IFlowModel
    {
        Task Finish();

        void Initialize(
            DissmissableDialogComponentViewModel flowViewModel,
            IFrameNavigationService frameNavigationService,
            RelayCommand<RelayCommand> navigateToFirstSscreenCommand);
    }
}

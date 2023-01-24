// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Playground.ViewModels;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Flows
{
    public abstract class FlowBase : IFlowModel
    {
        private TaskCompletionSource<bool> _finishCompletionSource;

        protected DissmissableDialogComponentViewModel FlowViewModel;
        protected IFrameNavigationService FrameNavigationService;

        public async Task Finish()
        {
            FlowViewModel.DialogComponent.CloseCommand.Execute(this);
            await _finishCompletionSource.Task;
            ((DissmissableDialogViewModelComponent)FlowViewModel.DialogComponent).Dismissed -= OnFlowDissmissed;
        }

        public void Initialize(
            DissmissableDialogComponentViewModel flowViewModel,
            IFrameNavigationService frameNavigationService,
            RelayCommand<RelayCommand> navigateToFirstSscreenCommand)
        {
            FlowViewModel = flowViewModel;
            FrameNavigationService = frameNavigationService;
            navigateToFirstSscreenCommand.Execute(new RelayCommand(NavigateToFirstStep));

            _finishCompletionSource = new TaskCompletionSource<bool>();
            ((DissmissableDialogViewModelComponent)FlowViewModel.DialogComponent).Dismissed += OnFlowDissmissed;
        }

        private void OnFlowDissmissed(object sender, EventArgs e)
        {
            _finishCompletionSource.TrySetResult(true);
            OnDissmissed();
        }

        protected virtual void OnDissmissed()
        {
        }

        protected abstract void NavigateToFirstStep();
    }
}

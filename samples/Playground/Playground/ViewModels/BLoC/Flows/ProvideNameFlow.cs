// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Playground.ViewModels.AddEditProfile;
using Playground.ViewModels.Flows;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Messenger;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.BLoC.Flows
{
    public class SaveNameMessage
    {
        public SaveNameMessage(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public class EditNameMessage
    {
        public EditNameMessage(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public class CloseMessage
    {
    }

    public class ProvideNameFlow : FlowBase
    {
        private readonly IFlowService _flowService;

        public ProvideNameFlow(IFlowService flowService)
        {
            _flowService = flowService;
        }

        protected override void NavigateToFirstStep()
        {
            Messenger.Default.Register<SaveNameMessage>(this, msg => NavigateToProfileDetailsScreen(msg.Name));
            Messenger.Default.Register<CloseMessage>(this, msg => this.Finish().FireAndForget());

            FrameNavigationService
                .For<BlocProvideNameViewModel>()
                .Navigate();
        }

        private void NavigateToProfileDetailsScreen(string name)
        {
            Messenger.Default.Register<EditNameMessage>(this, async msg => await OpenEditFlow(msg.Name));

            FrameNavigationService
                .For<BlocDetailViewModel>()
                .WithParam(x => x.Name, name)
                .Navigate();
        }

        private async Task OpenEditFlow(string name)
        {
            var editNameFlow = new EditNameFlow(name);
            await _flowService.ProcessAsync(editNameFlow);
        }

        protected override void OnDissmissed()
        {
            base.OnDissmissed();

            Messenger.Default.Unregister<SaveNameMessage>(this);
            Messenger.Default.Unregister<CloseMessage>(this);
            Messenger.Default.Unregister<EditNameMessage>(this);
        }
    }
}

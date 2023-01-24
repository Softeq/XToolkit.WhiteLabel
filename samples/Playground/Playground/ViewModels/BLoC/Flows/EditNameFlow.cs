// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.ViewModels.AddEditProfile;
using Playground.ViewModels.Flows;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Messenger;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.BLoC.Flows
{
    public class EditNameFlow : FlowBase
    {
        private readonly string _name;

        public EditNameFlow(string name)
        {
            _name = name;
        }

        protected override void NavigateToFirstStep()
        {
            Messenger.Default.Register<CloseMessage>(this, msg => Finish().FireAndForget());
            Messenger.Default.Register<SaveNameMessage>(this, msg => Finish().FireAndForget());

            FrameNavigationService.For<BlocProvideNameViewModel>()
                .WithParam(x => x.Name, _name)
                .Navigate();
        }

        protected override void OnDissmissed()
        {
            base.OnDissmissed();

            Messenger.Default.Unregister<SaveNameMessage>(this);
            Messenger.Default.Unregister<CloseMessage>(this);
        }
    }
}

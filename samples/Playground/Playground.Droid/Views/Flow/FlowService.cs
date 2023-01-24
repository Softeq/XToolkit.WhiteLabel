using System;
using System.Threading.Tasks;
using Android.Content;
using AndroidX.Core.App;
using Playground.ViewModels.Flows;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Droid.Views.Flow
{
    public class FlowService : IFlowService
    {
        private readonly IViewLocator _viewLocator;
        private readonly IContextProvider _contextProvider;
        private readonly IBackStackManager _backStackManager;
        private readonly IContainer _container;
        private IAnimationTransitionDelegateGetter _getter;

        public FlowService(
            IViewLocator viewLocator,
            IContextProvider contextProvider,
            IBackStackManager backStackManager,
            IContainer container)
        {
            _viewLocator = viewLocator;
            _contextProvider = contextProvider;
            _backStackManager = backStackManager;
            _container = container;
            TryResolveAnimationGetter();
        }

        public async Task ProcessAsync(IFlowModel flowModel)
        {
            var flowViewModel = _container.Resolve<FlowViewModel>();
            flowModel.Initialize(
                flowViewModel,
                flowViewModel.FrameNavigationService,
                new RelayCommand<RelayCommand>(x =>
                {
                    flowViewModel.NavigateToFirstScreenCommand = x;
                }));

            var type = _viewLocator.GetTargetType(flowViewModel.GetType(), ViewType.Activity);

            var intent = new Intent(_contextProvider.CurrentActivity, type);

            var animation = _getter?.GetCurrentAnimationDelegate2();
            if (animation != null)
            {
                var options = ActivityOptionsCompat.MakeSceneTransitionAnimation(_contextProvider.CurrentActivity, animation.Item1, animation.Item2);
                if (options != null)
                {
                    _contextProvider.CurrentActivity.StartActivity(intent, options.ToBundle());
                }
            }
            else
            {
                _contextProvider.CurrentActivity.StartActivity(intent);
            }

            _backStackManager.PushViewModel(flowViewModel);

            await flowViewModel.DialogComponent.Task;
        }

        private void TryResolveAnimationGetter()
        {
            try
            {
                _getter = Dependencies.Container.Resolve<IAnimationTransitionDelegateGetter>();
            }
            catch (Exception ex)
            {
                // Do nothing if can't resolve AnimationTransition
            }
        }
    }
}

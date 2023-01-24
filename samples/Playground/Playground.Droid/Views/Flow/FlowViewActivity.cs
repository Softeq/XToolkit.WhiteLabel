// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.App;
using Android.OS;
using Android.Transitions;
using Android.Views.Animations;
using Playground.ViewModels.Flows;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;

namespace Playground.Droid.Views.Flow
{
    [Activity]
    public class FlowViewActivity : ActivityBase<FlowViewModel>, IBindable
    {
        private FrameNavigationConfig _navigator;
        private FlowViewModel _vm;

        public object DataContext => ViewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Window.RequestFeature(Android.Views.WindowFeatures.ActivityTransitions);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            IAnimationTransitionDelegateGetter getter = TryGetAnimationGetter();
            var animation = getter?.GetCurrentAnimationDelegate2();
            if (animation != null)
            {
                OverridePendingTransition(0, 0);
                var window = Window;
                var set = new TransitionSet();
                set.SetOrdering(TransitionOrdering.Together);

                set.AddTransition(new ChangeBounds().SetInterpolator(new LinearInterpolator()).SetDuration(500));
                set.AddTransition(new ChangeImageTransform().SetInterpolator(new LinearInterpolator()).SetDuration(500));

                window.EnterTransition = set;
                window.SharedElementEnterTransition = set;
                window.SharedElementExitTransition = set;
                window.ExitTransition = set;
            }

            SetContentView(Resource.Layout.frames_activity);

            SupportActionBar?.Hide();

            if (!ViewModel.FrameNavigationService.IsInitialized)
            {
                _navigator = new FrameNavigationConfig(SupportFragmentManager, Resource.Id.activity_frames_layout);
                ViewModel.InitializeNavigation(_navigator);
            }

            PostponeEnterTransition();
        }

        private IAnimationTransitionDelegateGetter TryGetAnimationGetter()
        {
            try
            {
                return Dependencies.Container.Resolve<IAnimationTransitionDelegateGetter>();
            }
            catch
            {
                return null;
            }
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            ViewModel.DialogComponent.Closed += Dismiss;
        }

        public override void OnBackPressed()
        {
            if (ViewModel.FrameNavigationService.CanGoBack)
            {
                ViewModel.FrameNavigationService.GoBack();
            }
            else
            {
                ViewModel.DialogComponent.CloseCommand.Execute(null);
            }
        }

        protected override void DoDetachBindings()
        {
            base.DoDetachBindings();

            ViewModel.DialogComponent.Closed -= Dismiss;
        }

        private void Dismiss(object sender, EventArgs e)
        {
            Finish();
            ((DissmissableDialogViewModelComponent)ViewModel.DialogComponent).OnDismiss();
        }

        public void SetDataContext(object dataContext)
        {
            _vm = (FlowViewModel)dataContext;
            DoAttachBindings();
            _navigator = new FrameNavigationConfig(SupportFragmentManager, Resource.Id.activity_frames_layout);
            ViewModel.InitializeNavigation(_navigator);
        }

        protected override FlowViewModel ViewModel => _vm ?? base.ViewModel;
    }
}

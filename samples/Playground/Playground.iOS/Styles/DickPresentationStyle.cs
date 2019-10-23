﻿using System;
using CoreAnimation;
using CoreGraphics;
using Softeq.XToolkit.WhiteLabel.iOS.Dialogs;
using UIKit;

namespace Playground.iOS.Styles
{
    [PresentationStyle(Id = nameof(DickPresentationStyle))]
    public class DickPresentationStyle : PresentationArgsBase
    {
        public override UIModalPresentationStyle PresentationStyle => UIModalPresentationStyle.Custom;

        public override UIViewControllerTransitioningDelegate TransitioningDelegate => new DickViewControllerTransitioningDelegate();

        private class DickViewControllerTransitioningDelegate : UIViewControllerTransitioningDelegate
        {
            public override UIPresentationController GetPresentationControllerForPresentedViewController
                (UIViewController presentedViewController,
                UIViewController presentingViewController,
                UIViewController sourceViewController)
            {
                return new DickPresentationController(presentedViewController, presentingViewController);
            }

            public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController
                (UIViewController presented,
                UIViewController presenting,
                UIViewController source)
            {
                return new DickPresentationAnimator();
            }

            public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForDismissedController
                (UIViewController dismissed)
            {
                return new DickDismissAnimator();
            }

            private class DickPresentationController : UIPresentationController
            {
                public DickPresentationController(UIViewController presentedViewController,
                    UIViewController presentingViewController) : base(presentedViewController, presentingViewController)
                {

                }

                public override CGRect FrameOfPresentedViewInContainerView =>
                    new CGRect(UIScreen.MainScreen.Bounds.Width / 2 - 200,
                        UIScreen.MainScreen.Bounds.Height / 2 - 200,
                        400,
                        400);

                public override void ContainerViewWillLayoutSubviews()
                {
                    PresentedView.Frame = FrameOfPresentedViewInContainerView;
                    PresentedView.Layer.MasksToBounds = true;

                    var maskLayer = new CAShapeLayer
                    {
                        Path = GeneratePath()
                    };

                    PresentedView.Layer.Mask = maskLayer;
                }

                private CGPath GeneratePath()
                {
                    var path = new CGPath();
                    path.AddEllipseInRect(new CGRect(120, 0, 160, 300));
                    path.AddEllipseInRect(new CGRect(50, 200, 150, 150));
                    path.AddEllipseInRect(new CGRect(200, 200, 150, 150));
                    return path;
                }
            }

            private class DickPresentationAnimator : UIViewControllerAnimatedTransitioning
            {
                public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
                {
                    var toViewController = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);
                    var containerView = transitionContext.ContainerView;

                    var animationDuration = TransitionDuration(transitionContext);

                    var animation = (CASpringAnimation) CASpringAnimation.FromKeyPath("position.y");
                    animation.Damping = 10;
                    animation.InitialVelocity = 20;
                    animation.Mass = 1;
                    animation.Stiffness = 100;
                    animation.From = FromObject(containerView.Bounds.Height);
                    animation.To = FromObject(UIScreen.MainScreen.Bounds.Height / 2);
                    animation.Duration = TransitionDuration(transitionContext);
                    animation.AnimationStopped += delegate
                    {
                        transitionContext.CompleteTransition(true);
                    };

                    toViewController.View.Layer.AddAnimation(animation, null);

                    containerView.AddSubview(toViewController.View);
                }

                public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
                {
                    return 1;
                }
            }

            private class DickDismissAnimator : UIViewControllerAnimatedTransitioning
            {
                public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
                {
                    var fromViewController = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);
                    var containerView = transitionContext.ContainerView;

                    var animationDuration = TransitionDuration(transitionContext);

                    containerView.AddSubview(fromViewController.View);

                    UIView.Animate(animationDuration,
                        () => fromViewController.View.Transform = CGAffineTransform.MakeTranslation(0, containerView.Bounds.Height),
                        () => transitionContext.CompleteTransition(!transitionContext.TransitionWasCancelled));
                }

                public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
                {
                    return 1;
                }
            }
        }
    }
}

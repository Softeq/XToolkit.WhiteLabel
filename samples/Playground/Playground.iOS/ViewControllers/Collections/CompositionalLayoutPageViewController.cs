// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.iOS.ViewControllers.Collections.CompositionalLayout;
using Playground.ViewModels.Collections;
using Softeq.XToolkit.Common.iOS.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers.Collections
{
    public partial class CompositionalLayoutPageViewController : ViewControllerBase<CompositionalLayoutPageViewModel>
    {
        public CompositionalLayoutPageViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.Title = "Compositional Layout";

            var tabBarViewController = new UITabBarController
            {
                ViewControllers = new UIViewController[]
                {
                    new AdaptiveSectionsViewController()
                }
            };

            tabBarViewController.AddAsChildWithConstraints(this);
        }
    }
}

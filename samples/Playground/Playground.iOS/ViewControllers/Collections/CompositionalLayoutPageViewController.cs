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
    // Full Apple Sample:
    // https://developer.apple.com/documentation/uikit/views_and_controls/collection_views/using_collection_view_compositional_layouts_and_diffable_data_sources
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
                    new AdaptiveSectionsViewController
                    {
                        TabBarItem = new UITabBarItem("Adaptive Sections", null, 0)
                    },
                    new NestedGroupsViewController
                    {
                        TabBarItem = new UITabBarItem("Nested Groups", null, 0)
                    },
                }
            };

            tabBarViewController.AddAsChildWithConstraints(this);
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Forms.iOS.Renderers;
using Softeq.XToolkit.Common.iOS.Extensions;
using Softeq.XToolkit.WhiteLabel.Forms.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPageBase), typeof(TabbedPageBaseRenderer))]
namespace Playground.Forms.iOS.Renderers
{
    public class TabbedPageBaseRenderer : TabbedRenderer
    {
        public static void Initialize()
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            ResizeIcons();
        }

        public override void ItemSelected(UITabBar tabbar, UITabBarItem item)
        {
            ResizeIcons();
        }

        private void ResizeIcons()
        {
            if (Element is TabbedPage && TabBar?.Items?.Length > 0)
            {
                for (int i = 0; i < TabBar.Items.Length; i++)
                {
                    UpdateItem(TabBar.Items[i]);
                }
            }
        }

        protected virtual void UpdateItem(UITabBarItem item)
        {
            //item.Image = UIImageExtensions.MaxResizeImage(item.Image, 20, 20);
            //item.SelectedImage = UIImageExtensions.MaxResizeImage(item.SelectedImage, 20, 20);
        }
    }
}
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
        private const int IconSideSize = 20;

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
                    UpdateItemIconSize(TabBar.Items[i]);
                }
            }
        }

        protected virtual void UpdateItemIconSize(UITabBarItem item)
        {
            if (item.Image != null &&
               (item.Image.Size.Width != IconSideSize ||
                item.Image.Size.Height != IconSideSize))
            {
                item.Image = UIImageExtensions.MaxResizeImage(item.Image, IconSideSize, IconSideSize);
            }

            if (item.SelectedImage != null &&
               (item.SelectedImage.Size.Width != IconSideSize ||
                item.SelectedImage.Size.Height != IconSideSize))
            {
                item.SelectedImage = UIImageExtensions.MaxResizeImage(item.SelectedImage, IconSideSize, IconSideSize);
            }

        }
    }
}
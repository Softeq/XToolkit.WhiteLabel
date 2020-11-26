// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using CoreGraphics;
using Playground.Forms.iOS.Renderers;
using Softeq.XToolkit.WhiteLabel.Forms.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomTabbedPage), typeof(CustomTabbedPageRenderer))]
namespace Playground.Forms.iOS.Renderers
{
    public class CustomTabbedPageRenderer : TabbedRenderer
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
            if (TabBar?.Items == null)
            {
                return;
            }

            if (Element is TabbedPage tabs)
            {
                for (int i = 0; i < TabBar.Items.Length; i++)
                {
                    UpdateItem(TabBar.Items[i]);
                }
            }
        }

        private void UpdateItem(UITabBarItem item)
        {
            try
            {
                if (item == null)
                {
                    return;
                }

                if (item.Image != null)
                {
                    item.Image = MaxResizeImage(item.Image, 20, 20);
                }

                if (item.SelectedImage != null)
                {
                    item.SelectedImage = MaxResizeImage(item.SelectedImage, 20, 20);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("exception");
            }
        }

        public UIImage MaxResizeImage(UIImage sourceImage, float maxWidth, float maxHeight)
        {
            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
            if (maxResizeFactor > 1)
            {
                return sourceImage;
            }

            var width = maxResizeFactor * sourceSize.Width;
            var height = maxResizeFactor * sourceSize.Height;
            UIGraphics.BeginImageContext(new CGSize(width, height));
            sourceImage.Draw(new CGRect(0, 0, width, height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage;
        }
    }
}
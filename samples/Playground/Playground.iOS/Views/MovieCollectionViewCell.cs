﻿using System;
using FFImageLoading;
using Foundation;
using Playground.ViewModels.Pages;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using Softeq.XToolkit.Bindings.iOS.Bindable.Abstract;
using UIKit;

namespace Playground.iOS.Views
{
    public partial class MovieCollectionViewCell : BindableCollectionViewCell<ItemModel>
    {
        public static readonly NSString Key = new NSString(nameof(MovieCollectionViewCell));
        public static readonly UINib Nib;

        static MovieCollectionViewCell()
        {
            Nib = UINib.FromName(Key, NSBundle.MainBundle);
        }

        protected MovieCollectionViewCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(() =>
            {
                ImageService.Instance.LoadUrl(DataContext.IconUrl).Into(Poster);
                Title.Text = DataContext.Title;
                Description.Text = DataContext.Description;
            });
        }
    }
}

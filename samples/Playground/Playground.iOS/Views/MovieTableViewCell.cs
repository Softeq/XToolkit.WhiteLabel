﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using FFImageLoading;
using Foundation;
using Playground.Models;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using UIKit;

namespace Playground.iOS.Views
{
    public partial class MovieTableViewCell : BindableTableViewCell<ItemViewModel>
    {
        public static readonly NSString Key = new NSString("MovieTableViewCell");
        public static readonly UINib Nib;

        static MovieTableViewCell()
        {
            Nib = UINib.FromName("MovieTableViewCell", NSBundle.MainBundle);
        }

        protected MovieTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void DoAttachBindings()
        {
            base.DoAttachBindings();

            ImageService.Instance.LoadUrl(ViewModel.IconUrl).Into(Poster);

            this.Bind(() => ViewModel.Title, () => Title.Text);
        }

        public override void DoDetachBindings()
        {
            base.DoDetachBindings();

            Poster.Image = null;
        }
    }
}


﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS
{
    /// <summary>
    ///     Based on <see cref="T:UIKit.UIViewController"/>, used for creating ViewControllers.
    /// </summary>
    public abstract class ViewControllerBase : UIViewController
    {
        protected ViewControllerBase()
        {
        }

        protected internal ViewControllerBase(IntPtr handle)
            : base(handle)
        {
        }

        public List<IViewControllerComponent> ControllerComponents { get; } = new List<IViewControllerComponent>();
    }

    /// <summary>
    ///     Generic class based on <see cref="T:UIKit.UIViewController"/>, used for creating ViewControllers.
    /// </summary>
    /// <typeparam name="TViewModel">Type of ViewModel.</typeparam>
    public abstract class ViewControllerBase<TViewModel> : ViewControllerBase, IBindable
        where TViewModel : IViewModelBase
    {
        protected ViewControllerBase()
        {
            InitViewController();
        }

        protected internal ViewControllerBase(IntPtr handle)
            : base(handle)
        {
            InitViewController();
        }

        public List<Binding> Bindings { get; } = new List<Binding>();

        public object DataContext { get; private set; } = default!;

        public TViewModel ViewModel => (TViewModel) DataContext;

        void IBindable.SetDataContext(object dataContext)
        {
            DataContext = dataContext;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ViewModel.OnInitialize();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ViewModel.OnAppearing();
            AttachBindings();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            DetachBindings();
            ViewModel.OnDisappearing();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            CloseDialogIfNeeded();
        }

        protected virtual void DoAttachBindings()
        {
        }

        protected virtual void DoDetachBindings()
        {
        }

        private void AttachBindings()
        {
            DoAttachBindings();
        }

        private void DetachBindings()
        {
            BindableExtensions.DetachBindings(this);
            DoDetachBindings();
        }

        private void InitViewController()
        {
            ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
        }

        private void CloseDialogIfNeeded()
        {
            if (ViewModel is DialogViewModelBase dialogViewModel)
            {
                if (IsBeingDismissed || IsMovingFromParentViewController)
                {
                    dialogViewModel.DialogComponent.CloseCommand.Execute(null);
                }
            }
        }
    }
}

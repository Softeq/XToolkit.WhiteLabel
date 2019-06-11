// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using UIKit;
using Softeq.XToolkit.Bindings.Abstract;

namespace Softeq.XToolkit.WhiteLabel.iOS
{
    public abstract class ViewControllerBase : UIViewController
    {
        public abstract void SetExistingViewModel(object viewModel);

        protected ViewControllerBase()
        {
        }

        protected internal ViewControllerBase(IntPtr handle) : base(handle)
        {
        }

        public List<IViewControllerComponent> ControllerComponents { get; } = new List<IViewControllerComponent>();
    }

    public abstract class ViewControllerBase<TViewModel> : ViewControllerBase, IBindable where TViewModel : IViewModelBase
    {
        protected ViewControllerBase()
        {
        }

        protected internal ViewControllerBase(IntPtr handle) : base(handle)
        {
        }

        public TViewModel ViewModel { get; private set; }

        public object DataContext => ViewModel;

        public List<Binding> Bindings { get; } = new List<Binding>();

        public override void SetExistingViewModel(object viewModel)
        {
            ViewModel = (TViewModel) viewModel;
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
            Bindings.DetachAllAndClear();
            DoDetachBindings();
        }
    }
}

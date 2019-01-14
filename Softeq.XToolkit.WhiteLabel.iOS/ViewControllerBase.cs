// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using UIKit;

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

    public abstract class ViewControllerBase<TViewModel> : ViewControllerBase where TViewModel : IViewModelBase
    {
        protected ViewControllerBase()
        {
        }

        protected internal ViewControllerBase(IntPtr handle) : base(handle)
        {
        }

        public TViewModel ViewModel { get; private set; }

        protected IList<Binding> Bindings { get; } = new List<Binding>();

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

        protected void Bind<T1, T2>(
            Expression<Func<T1>> sourcePropertyExpression,
            Expression<Func<T2>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.OneWay,
            IConverter<T2, T1> converter = null)
        {
            Bindings.Add(this.SetBinding(sourcePropertyExpression, targetPropertyExpression, mode)
                .SetConverter(converter));
        }

        protected void Bind<T1, T2>(Expression<Func<T1>> sourcePropertyExpression,
            Expression<Func<T2>> targetPropertyExpression, IConverter<T2, T1> converter)
        {
            Bind(sourcePropertyExpression, targetPropertyExpression, BindingMode.OneWay, converter);
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
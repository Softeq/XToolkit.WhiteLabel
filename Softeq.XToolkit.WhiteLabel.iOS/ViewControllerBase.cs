// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Interfaces;
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
        private IList<object> _converters { get; } = new List<object>();

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

        protected void Bind<T1, T2>(Expression<Func<T1>> sourcePropertyExpression,
            Expression<Func<T2>> targetPropertyExpression = null, BindingMode mode = BindingMode.OneWay,
            IValueConverter<T1, T2> converter = null)
        {
            var binding = this.SetBinding(sourcePropertyExpression, targetPropertyExpression, mode);
            SetConverters(binding, converter);
            Bindings.Add(binding);
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
            _converters.Clear();
            DoDetachBindings();
        }

        private void SetConverters<T1, T2>(Binding<T1, T2> binding, IValueConverter<T1, T2> converter)
        {
            if (converter == null)
            {
                return;
            }

            if (!_converters.Contains(converter))
            {
                _converters.Add(converter);
            }

            binding.ConvertSourceToTarget(converter.Convert);
            binding.ConvertTargetToSource(converter.ConvertBack);
        }
    }
}
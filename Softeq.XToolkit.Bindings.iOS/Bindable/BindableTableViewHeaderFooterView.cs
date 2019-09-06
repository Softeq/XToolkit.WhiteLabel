// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Bindable
{
    public abstract class BindableTableViewHeaderFooterView<TItem> : UITableViewHeaderFooterView, IBindableView
    {
        protected BindableTableViewHeaderFooterView()
        {
            Initialize();
        }

        protected BindableTableViewHeaderFooterView(NSCoder coder)
            : base(coder)
        {
            Initialize();
        }

        protected BindableTableViewHeaderFooterView(CGRect frame)
            : base(frame)
        {
            Initialize();
        }

        protected BindableTableViewHeaderFooterView(NSObjectFlag t)
            : base(t)
        {
            Initialize();
        }

        protected internal BindableTableViewHeaderFooterView(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }

        private void Initialize()
        {
            Bindings = new List<Binding>();

            OnInitialize();
        }

        public List<Binding> Bindings { get; private set; }

        public object DataContext { get; private set; }

        protected TItem ViewModel => (TItem) DataContext;

        void IBindable.SetDataContext(object dataContext)
        {
            DataContext = dataContext;
        }

        protected virtual void OnInitialize()
        {
        }

        /// <inheritdoc />
        public virtual void DoAttachBindings()
        {
        }

        /// <inheritdoc />
        public virtual void DoDetachBindings()
        {
            this.DetachBindings();
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Bindable
{
    public abstract class BindableTableViewCell<TItem> : UITableViewCell, IBindableView
    {
        protected BindableTableViewCell(IntPtr handle) : base(handle)
        {
        }

        public List<Binding> Bindings { get; } = new List<Binding>();

        public object DataContext { get; private set; }

        protected TItem ViewModel => (TItem) DataContext;

        void IBindable.SetDataContext(object context)
        {
            DataContext = context;
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

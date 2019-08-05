// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Bindable
{
    public abstract class BindableHeaderCell<TItem> : UITableViewHeaderFooterView, IBindable
    {
        protected BindableHeaderCell(IntPtr handle) : base(handle)
        {
        }

        public List<Binding> Bindings { get; } = new List<Binding>();

        public object DataContext { get; set; }

        protected TItem ViewModel => (TItem) DataContext;

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
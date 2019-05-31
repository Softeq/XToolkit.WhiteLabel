// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.Bindings.iOS.Bindable.Abstract;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Bindable
{
    public abstract class BindableHeaderCell<T> : UITableViewHeaderFooterView, IBindable
    {
        public T DataContext => (T) BindingContext;

        protected BindableHeaderCell(IntPtr handle) : base(handle)
        {
            Bindings = new List<Binding>();
        }

        public object BindingContext { get; set; }
        public List<Binding> Bindings { get; }
        public Action Activator { get; set; }
    }
}
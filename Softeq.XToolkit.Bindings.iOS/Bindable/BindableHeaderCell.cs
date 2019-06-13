// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.Bindings.Abstract;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Bindable
{
    public abstract class BindableHeaderCell<TViewModel> : UITableViewHeaderFooterView, IBindable
    {
        protected BindableHeaderCell(IntPtr handle) : base(handle)
        {
            Bindings = new List<Binding>();
        }

        protected TViewModel ViewModel => (TViewModel) DataContext;

        public object DataContext { get; set; }
        public List<Binding> Bindings { get; }

        public abstract void SetBindings();
    }
}
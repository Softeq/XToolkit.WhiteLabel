// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using UIKit;
using Softeq.XToolkit.Bindings.Abstract;

namespace Softeq.XToolkit.Bindings.iOS.Bindable
{
    public abstract class BindableCollectionViewCell<TViewModel> : UICollectionViewCell, IBindableOwner
    {
        protected BindableCollectionViewCell(IntPtr handle) : base(handle)
        {
            Bindings = new List<Binding>();
        }

        protected TViewModel ViewModel => (TViewModel) DataContext;

        public object DataContext { get; set; }
        public List<Binding> Bindings { get; }

        public abstract void SetBindings();
    }

    public abstract class BindableTableViewCell<TViewModel> : UITableViewCell, IBindableOwner
    {
        protected BindableTableViewCell(IntPtr handle) : base(handle)
        {
            Bindings = new List<Binding>();
        }

        protected TViewModel ViewModel => (TViewModel) DataContext;

        public object DataContext { get; set; }
        public List<Binding> Bindings { get; }

        public abstract void SetBindings();
    }
}

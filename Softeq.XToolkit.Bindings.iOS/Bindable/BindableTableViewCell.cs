// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System;
using System.Collections.Generic;
using Softeq.XToolkit.Bindings.iOS.Bindable.Abstract;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Bindable
{
    public abstract class BindableCollectionViewCell<T> : UICollectionViewCell, IBindable
    {
        protected BindableCollectionViewCell(IntPtr handle) : base(handle)
        {
            Bindings = new List<Binding>();
        }

        public object BindingContext { get; set; }

        public T DataContext => (T) BindingContext;

        public List<Binding> Bindings { get; }
        public Action Activator { get; set; }
    }

    public abstract class BindableTableViewCell<T> : UITableViewCell, IBindable
    {
        protected BindableTableViewCell(IntPtr handle) : base(handle)
        {
            Bindings = new List<Binding>();
        }

        public T DataContext => (T) BindingContext;
        public object BindingContext { get; set; }
        public List<Binding> Bindings { get; }
        public Action Activator { get; set; }
    }
}
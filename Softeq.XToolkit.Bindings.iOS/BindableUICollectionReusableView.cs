// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS
{
    public class BindableUICollectionReusableView<T> : UICollectionReusableView, IBindableView
    {
        public BindableUICollectionReusableView(IntPtr handle) : base(handle)
        {
        }

        public List<Binding> Bindings { get; } = new List<Binding>();

        public object DataContext { get; private set; }

        protected T ViewModel => (T) DataContext;

        public virtual void DoAttachBindings()
        {
        }

        public virtual void DoDetachBindings()
        {
            this.DetachBindings();
        }

        void IBindable.SetDataContext(object dataContext)
        {
            DataContext = dataContext;
        }
    }
}

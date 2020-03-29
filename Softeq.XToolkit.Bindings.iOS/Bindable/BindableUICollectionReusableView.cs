// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using UIKit;

#nullable disable

namespace Softeq.XToolkit.Bindings.iOS.Bindable
{
    public class BindableUICollectionReusableView<T> : UICollectionReusableView, IBindableView
    {
        public BindableUICollectionReusableView()
        {
            Initialize();
        }

        public BindableUICollectionReusableView(NSCoder coder)
            : base(coder)
        {
            Initialize();
        }

        public BindableUICollectionReusableView(CGRect frame)
            : base(frame)
        {
            Initialize();
        }

        protected BindableUICollectionReusableView(NSObjectFlag t)
            : base(t)
        {
            Initialize();
        }

        protected BindableUICollectionReusableView(IntPtr handle)
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

        protected T ViewModel => (T) DataContext;

        void IBindable.SetDataContext(object dataContext)
        {
            DataContext = dataContext;
        }

        protected virtual void OnInitialize()
        {
        }

        public virtual void DoAttachBindings()
        {
        }

        public virtual void DoDetachBindings()
        {
            this.DetachBindings();
        }
    }
}

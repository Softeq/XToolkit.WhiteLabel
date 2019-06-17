// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using CoreGraphics;
using Foundation;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Bindable.CollectionView
{
    [Register(nameof(BindableCollectionView))]
    public class BindableCollectionView : UICollectionView
    {
        private IBindableCollectionViewSource _source;

        public BindableCollectionView(NSCoder coder) : base(coder)
        {
            InitInternal();
        }

        protected BindableCollectionView(NSObjectFlag t) : base(t)
        {
            InitInternal();
        }

        protected internal BindableCollectionView(IntPtr handle) : base(handle)
        {
            InitInternal();
        }

        public BindableCollectionView(CGRect frame, UICollectionViewLayout layout) : base(frame, layout)
        {
            InitInternal();
        }

        public new IUICollectionViewDataSource DataSource
        {
            get => base.DataSource;
            set
            {
                base.DataSource = value;
                _source = (IBindableCollectionViewSource) value;
            }
        }

        public new UICollectionViewSource Source
        {
            get => base.Source;
            set
            {
                base.Source = value;
                _source = (IBindableCollectionViewSource) value;
            }
        }

        public override UICollectionReusableView DequeueReusableCell(NSString reuseIdentifier, NSIndexPath indexPath)
        {
            var cell = base.DequeueReusableCell(reuseIdentifier, indexPath);

            var bindableCell = (IBindable) cell;
            bindableCell.SetDataContext(_source.GetItemAt(indexPath.Row));
            
            return cell;
        }

        public override UICollectionReusableView DequeueReusableSupplementaryView(NSString kind, NSString identifier,
            NSIndexPath indexPath)
        {
            var cell = base.DequeueReusableSupplementaryView(kind, identifier, indexPath);

            var bindableCell = (IBindable) cell;
            bindableCell.SetDataContext(_source.GetItemAt(indexPath.Row));
            
            return cell;
        }

        private void InitInternal()
        {
            Delegate = new BindableUICollectionViewDelegateFlowLayout();
        }
    }
}
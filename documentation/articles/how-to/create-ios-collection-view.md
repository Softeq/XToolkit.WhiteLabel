# Create iOS CollectionView

> This page actual for WhiteLabel v2.0.0-beta2+

1. Create empty CollectionViewCell
2. Add inheritance of [BindableCollectionViewCell&lt;TItem&gt;](xref:Softeq.XToolkit.Bindings.iOS.Bindable.BindableCollectionViewCell`1)

```cs
public partial class ItemViewCell : BindableCollectionViewCell<ItemViewModel>
{
    public static readonly NSString Key = new NSString(nameof(ItemViewCell));
    public static readonly UINib Nib;

    static ItemViewCell() => Nib = UINib.FromName(Key, NSBundle.MainBundle);

    protected ItemViewCell(IntPtr handle) : base(handle)
    {
    }

    public override void SetBindings()
    {
    }
}
```

3. Add [UICollectionView](https://developer.apple.com/documentation/uikit/uicollectionview) to your Storyboard;
4. Setup UICollectionView:

```cs
CollectionView.RegisterNibForCell(ItemViewCell.Nib, ItemViewCell.Key);
CollectionView.DataSource = new BindableCollectionViewSource<ItemViewModel, ItemViewCell>(ViewModel.Items);
```

### Handle item clicks

```cs
// ...
CollectionView.Delegate = new BindableUICollectionViewDelegateFlowLayout();
CollectionView.DataSource = new BindableCollectionViewSource<ItemViewModel, ItemViewCell>(ViewModel.Items)
{
    ItemClick = ViewModel.SelectItemCommand
};
```

### Bind SelectionItem

// TODO:

### Groups

// TODO:

---

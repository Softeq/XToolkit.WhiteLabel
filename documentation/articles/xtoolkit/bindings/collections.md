# Collections

## iOS

All `Binbable*` sources work with reusable cells, for that must be declared before use:

- Registering cell as reusable:
`Table\CollectionView.RegisterNibForCellReuse(ItemViewCell.Nib, ItemViewCell.Key);`
- Inherit `BindableTableViewCell`, `BindableCollectionViewCell`, `BindableTableViewHeaderFooterView`, `BindableUICollectionReusableView` or any custom cell with `IBindableView` interface.

More samples about using those sources you can see in our [Playground](https://github.com/Softeq/XToolkit.WhiteLabel/tree/master/samples/Playground/Playground.iOS/ViewControllers/Collections).

### UITableViewSource

- [BindableTableViewSource<TItem, TItemCell>](xref:Softeq.XToolkit.Bindings.iOS.Bindable.BindableTableViewSource`2)

```cs
var source = new BindableTableViewSource<ItemViewModel, ItemViewCell>(ViewModel.Items);
```

- [BindableTableViewSource<TKey, TItem, TGroupCell, TItemCell>](xref:Softeq.XToolkit.Bindings.iOS.Bindable.BindableTableViewSource`4)

```cs
var source = new BindableTableViewSource<HeaderViewModel, ItemViewModel, HeaderViewCell, ItemViewCell>(TableView, ViewModel.Items);
```

### UICollectionViewSource

- [BindableCollectionViewSource<TItem, TCell>](xref:Softeq.XToolkit.Bindings.iOS.Bindable.BindableCollectionViewSource`2)

```cs
var source = new BindableCollectionViewSource<ItemViewModel, ItemViewCell>(ViewModel.Items);
```

## Android

All `Binbable*` adapters work with reusable ViewHolders, for that must be declared before use:

- Inherit [BindableViewHolder](xref:Softeq.XToolkit.Bindings.Droid.Bindable.BindableViewHolder`1), or any custom ViewHolder with [IBindableViewHolder](xref:Softeq.XToolkit.Bindings.Droid.Bindable.IBindableViewHolder) interface.
- Add [BindableViewHolderLayout](xref:Softeq.XToolkit.Bindings.Droid.Bindable.BindableViewHolderLayoutAttribute) attribute for auto-inflating ViewHolder layout.

More samples about using those sources you can see in our [Playground](https://github.com/Softeq/XToolkit.WhiteLabel/tree/master/samples/Playground/Playground.Droid/Views/Collections).

- [BindableRecyclerViewAdapter<TViewModel, TViewHolder>](xref:Softeq.XToolkit.Bindings.Droid.Bindable.BindableRecyclerViewAdapter`2)

```cs
var adapter = new BindableRecyclerViewAdapter<ItemViewModel, ItemViewHolder>(ViewModel.Items);
```

- [BindableRecyclerViewAdapter<TKey, TItem, TItemHolder>](xref:Softeq.XToolkit.Bindings.Droid.Bindable.BindableRecyclerViewAdapter`3)

```cs
var adapter = new BindableRecyclerViewAdapter<HeaderViewModel, ItemViewModel, ItemViewHolder>(
    items: ViewModel.Items,
    headerViewHolder: typeof(HeaderViewHolder));
```

---

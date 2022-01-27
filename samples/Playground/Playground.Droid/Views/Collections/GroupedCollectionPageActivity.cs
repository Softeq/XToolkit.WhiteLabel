// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Playground.ViewModels.Collections;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Droid.Converters;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.Collections
{
    [Activity]
    public class GroupedCollectionPageActivity : ActivityBase<GroupedTablePageViewModel>
    {
        private const int ColumnsCount = 3;

        private ProgressBar _progress = null!;
        private RecyclerView _recyclerView = null!;
        private Button _generateButton = null!;
        private Button _addButton = null!;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_group_collection);

            _generateButton = FindViewById<Button>(Resource.Id.button_generate)!;
            _addButton = FindViewById<Button>(Resource.Id.button_add)!;
            _progress = FindViewById<ProgressBar>(Resource.Id.activity_group_collection_progress)!;
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.activity_group_collection_lst)!;

            // use this setting to improve performance if you know that changes
            // in content do not change the layout size of the RecyclerView
            _recyclerView.HasFixedSize = true;

            // init adapter
            var adapter = new CustomAdapter(ViewModel.ProductListViewModel.Products, typeof(ProductHeaderViewHolder), typeof(ProductFooterViewHolder))
            {
                // ItemClick = ViewModel.AddToCartCommand
            };
            _recyclerView.SetAdapter(adapter);

            // init layout
            var layoutManager = new GridLayoutManager(this, ColumnsCount);
            layoutManager.SetSpanSizeLookup(new GroupedSpanSizeLookup(adapter, ColumnsCount));
            _recyclerView.SetLayoutManager(layoutManager);

            _generateButton.SetCommand(ViewModel.GenerateGroupCommand);
            _addButton.SetCommand(ViewModel.AddAllToCartCommand);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.ProductBasketViewModel.Status, () => SupportActionBar.Title);
            this.Bind(() => ViewModel.ProductListViewModel.IsBusy, () => _progress.Visibility, VisibilityConverter.Gone);
        }
    }
}

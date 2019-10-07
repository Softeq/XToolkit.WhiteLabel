// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Playground.ViewModels;
using Softeq.XToolkit.Bindings.Droid;
using Softeq.XToolkit.Common.Weak;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.Droid.Views
{
    [Activity]
    public class MainPageActivity : ActivityBase<MainPageViewModel>
    {
        private ExpandableListView _listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            Title = ViewModel.Title;

            _listView = FindViewById<ExpandableListView>(Resource.Id.main_page_list_view);
            _listView.SetAdapter(new ObservableGroupAdapter<string, CommandAction>(
                ViewModel.Items,
                _listView,
                (groupIndex, itemIndex, isLastItem, convertView, listView, item) =>
                {
                    var view = convertView ?? LayoutInflater.FromContext(listView.Context)
                                   .Inflate(Resource.Layout.item_main, null);

                    var textView = view.FindViewById<TextView>(Resource.Id.item_view_text_view);
                    textView.Text = item.Title;

                    view.SetOnClickListener(new OnItemClickListener(item));

                    return view;
                },
                (groupIndex, isExpanded, convertView, listView, item) =>
                {
                    var view = convertView ?? LayoutInflater.FromContext(listView.Context)
                                   .Inflate(Resource.Layout.item_main_header, null);
                    var textView = view.FindViewById<TextView>(Resource.Id.group_header_view_text_view);
                    textView.Text = item;

                    return view;
                }));
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.action_empty)
            {
                ViewModel.GoToEmptyCommand.Execute(null);
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private class OnItemClickListener : Object, View.IOnClickListener
        {
            private readonly WeakReferenceEx<CommandAction> _viewModelRef;

            public OnItemClickListener(CommandAction viewModel)
            {
                _viewModelRef = WeakReferenceEx.Create(viewModel);
            }

            public void OnClick(View v)
            {
                _viewModelRef.Target?.Command.Execute(v);
            }
        }
    }
}

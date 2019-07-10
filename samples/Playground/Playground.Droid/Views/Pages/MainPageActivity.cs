// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Softeq.XToolkit.Bindings.Droid;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Playground.ViewModels.Pages;

namespace Playground.Droid.Views.Pages
{
    [Activity]
    public class MainPageActivity : ActivityBase<MainPageViewModel>
    {
        private ExpandableListView _listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.main_page);

            _listView = FindViewById<ExpandableListView>(Resource.Id.main_page_list_view);
            _listView.SetAdapter(new ObservableGroupAdapter<string, CommandAction>(
                ViewModel.Items,
                _listView,
                (groupIndex, itemIndex, isLastItem, convertView, listView, item) =>
                {
                    var view = convertView ?? LayoutInflater.FromContext(listView.Context)
                                   .Inflate(Resource.Layout.item_view, null);

                    var textView = view.FindViewById<TextView>(Resource.Id.item_view_text_view);
                    textView.Text = item.Title;

                    view.SetOnClickListener(new OnItemClickListener(item));

                    return view;
                },
                (groupIndex, isExpanded, convertView, listView, item) =>
                {
                    var view = convertView ?? LayoutInflater.FromContext(listView.Context)
                                   .Inflate(Resource.Layout.group_header_view, null);
                    var textView = view.FindViewById<TextView>(Resource.Id.group_header_view_text_view);
                    textView.Text = item;

                    return view;
                }));
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
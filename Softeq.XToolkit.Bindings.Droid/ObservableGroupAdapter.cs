// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.Views;
using Android.Widget;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.WeakSubscription;
using Object = Java.Lang.Object;

namespace Softeq.XToolkit.Bindings.Droid
{
    public class ObservableGroupAdapter<TKey, TValue> : BaseExpandableListAdapter
    {
        private readonly WeakReferenceEx<ExpandableListView> _parent;
        private ObservableKeyGroupsCollection<TKey, TValue> _dataSource;
        private Func<int, int, bool, View, ViewGroup, TValue, View> _getChildViewFunc;
        private Func<int, bool, View, ViewGroup, TKey, View> _getGroupViewFunc;
        private IDisposable _subscription;

        public ObservableGroupAdapter(
            ObservableKeyGroupsCollection<TKey, TValue> items,
            ExpandableListView parent,
            Func<int, int, bool, View, ViewGroup, TValue, View> getChildViewFunc,
            Func<int, bool, View, ViewGroup, TKey, View> getGroupViewFunc)
        {
            _dataSource = items;
            _parent = WeakReferenceEx.Create(parent);
            _getChildViewFunc = getChildViewFunc;
            _getGroupViewFunc = getGroupViewFunc;
            _subscription = new NotifyCollectionChangedEventSubscription(_dataSource, OnCollectionChanged);
        }

        public override int GroupCount => _dataSource.Count;

        public override bool HasStableIds => true;

        public override Object GetChild(int groupPosition, int childPosition)
        {
            return null;
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return _dataSource[groupPosition].Count;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView,
            ViewGroup parent)
        {
            return _getChildViewFunc(groupPosition, childPosition, isLastChild, convertView, parent,
                _dataSource[groupPosition][childPosition]);
        }

        public override Object GetGroup(int groupPosition)
        {
            return null;
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            return _getGroupViewFunc(groupPosition, isExpanded, convertView, parent, _dataSource[groupPosition].Key);
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _subscription?.Dispose();
            _dataSource = null;
            _getGroupViewFunc = null;
            _getChildViewFunc = null;
        }

        private void OnCollectionChanged(object sender, EventArgs e)
        {
            NotifyDataSetChanged();

            for (var i = 0; i < _dataSource.Count; i++)
            {
                _parent.Target?.ExpandGroup(i);
            }
        }
    }
}
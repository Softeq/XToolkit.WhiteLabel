// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Android.Views;

namespace Softeq.XToolkit.Bindings.Droid
{
    public class ObservableAdapterExtended<T> : ObservableAdapter<T>
    {
        private readonly Func<int, T, View, ViewGroup, int, View> _getTemplateDelegateExtended;

        public ObservableAdapterExtended(IList<T> list, Func<int, T, View, ViewGroup, int, View> getTemplateDelegate)
        {
            DataSource = list;
            _getTemplateDelegateExtended = getTemplateDelegate;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (_getTemplateDelegateExtended == null)
            {
                return convertView;
            }

            var arg = DataSource[position];
            return _getTemplateDelegateExtended(position, arg, convertView, parent, Count);
        }
    }
}
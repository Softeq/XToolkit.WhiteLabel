// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Controls
{
    public class SelectorModel<T> : UIPickerViewModel
    {
        private readonly Dictionary<T, string> _items;

        public SelectorModel(Dictionary<T, string> dictionary)
        {
            _items = dictionary;
            SelectedValue = _items.First().Key;
        }

        public T SelectedValue { get; set; }

        public event Action<T>? ValueChanged;

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return _items.Count;
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            var v = _items.Values.ToArray()[row];
            return v;
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            var v = _items.Keys.ToArray()[row];
            SelectedValue = v;
            ValueChanged?.Invoke(v);
        }
    }
}
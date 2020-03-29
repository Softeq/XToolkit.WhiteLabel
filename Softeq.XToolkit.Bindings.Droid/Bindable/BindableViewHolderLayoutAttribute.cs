// // Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Bindings.Droid.Bindable
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BindableViewHolderLayoutAttribute : Attribute
    {
        public BindableViewHolderLayoutAttribute(int layoutId)
        {
            LayoutId = layoutId;
        }

        public int LayoutId { get; }
    }
}

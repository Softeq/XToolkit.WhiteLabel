using System;
using Softeq.XToolkit.Bindings.Abstract;

namespace Softeq.XToolkit.Bindings.Droid.Bindable
{
    public interface IBindableViewHolder : IBindable
    {
        event EventHandler ItemClicked;

        void OnAttachedToWindow();
        void OnDetachedFromWindow();
        void OnViewRecycled();
    }
}

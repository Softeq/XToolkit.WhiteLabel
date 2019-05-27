// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;

namespace Softeq.XToolkit.Bindings.iOS.Bindable.Abstract
{
    public interface IBindable
    {
        object BindingContext { get; set; }
        List<Binding> Bindings { get; }
        Action Activator { get; set; }
    }
}
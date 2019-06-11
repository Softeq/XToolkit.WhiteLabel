using System.Collections.Generic;

namespace Softeq.XToolkit.Bindings.Abstract
{
    public interface IBindable
    {
        object DataContext { get; }
        List<Binding> Bindings { get; }
    }
}

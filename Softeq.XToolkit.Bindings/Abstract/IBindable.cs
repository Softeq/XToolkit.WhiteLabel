using System.Collections.Generic;

namespace Softeq.XToolkit.Bindings.Abstract
{
    public interface IBindable
    {
        List<Binding> Bindings { get; }
    }
}

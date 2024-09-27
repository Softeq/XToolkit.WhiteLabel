// Developed for PAWS-HALO by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Bindings.Droid.Bindable;

public interface IBindableRecyclerViewAdapter
{
    void DoAttachBindings();
    void DoDetachBindings();
    void CleanUp();
}

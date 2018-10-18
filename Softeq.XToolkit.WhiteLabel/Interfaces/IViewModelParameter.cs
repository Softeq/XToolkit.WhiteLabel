// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Interfaces
{
    public interface IViewModelParameter<in T>
    {
        T Parameter { set; }
    }
}
// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Interfaces
{
    public interface ILogManager
    {
        ILogger GetLogger<T>();
        ILogger GetLogger(string name);
    }
}
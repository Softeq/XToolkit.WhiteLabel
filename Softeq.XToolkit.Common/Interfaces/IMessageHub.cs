// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Interfaces
{
    public interface IMessageHub
    {
        void SendMessage<T>(T message);
    }
}
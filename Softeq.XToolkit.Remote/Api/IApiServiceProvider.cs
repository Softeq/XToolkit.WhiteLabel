using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote.Api
{
    public interface IApiServiceProvider<out TApiService>
    {
        TApiService Get();

        TApiService GetByPriority(RequestPriority requestPriority);
    }
}

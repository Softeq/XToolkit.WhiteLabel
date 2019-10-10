namespace Softeq.XToolkit.Remote
{
    public interface IRemoteServiceFactory
    {
        IRemoteService<T> Create<T>(RemoteServiceConfig config);
    }

    public class RestRemoteServiceFactory : IRemoteServiceFactory
    {
        public IRemoteService<T> Create<T>(RemoteServiceConfig config)
        {
            var refitService = new RefitService<T>(
                config.BaseUrl,
                config.AutoRedirectRequests,
                config.DelegatingHandler,
                config.DefaultHeaders);

            return new RestRemoteService<T>(refitService);
        }
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net.Http;

namespace Softeq.XToolkit.Remote
{
    /// <summary>
    ///     A factory abstraction for a component that can create <see cref="IRemoteService{T}"/> instances
    ///     with custom configuration.
    /// </summary>
    public interface IRemoteServiceFactory
    {
        /// <summary>
        ///     Creates an instance of <see cref="IRemoteService{T}"/> for <paramref name="baseUrl"/>.
        /// </summary>
        /// <param name="baseUrl">The base address used when sending requests.</param>
        /// <typeparam name="T">Type of Refit interface.</typeparam>
        /// <returns>New instance of <see cref="IRemoteService{T}"/>.</returns>
        IRemoteService<T> Create<T>(string baseUrl);

        /// <summary>
        ///     Creates an instance of <see cref="IRemoteService{T}"/> for <paramref name="httpClient"/>.
        /// </summary>
        /// <param name="httpClient">Custom instance of <see cref="T:System.Net.Http.HttpClient"/>.</param>
        /// <typeparam name="T">Type of Refit interface.</typeparam>
        /// <returns>New instance of <see cref="IRemoteService{T}"/>.</returns>
        IRemoteService<T> Create<T>(HttpClient httpClient);
    }
}

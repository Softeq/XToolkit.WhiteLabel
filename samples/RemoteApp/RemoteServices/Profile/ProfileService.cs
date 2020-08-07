// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteServices.Profile
{
    public class ProfileService
    {
        private readonly ProfileRemoteService _remoteService;

        public ProfileService(
            ProfileRemoteService remoteService)
        {
            _remoteService = remoteService;
        }

        public async Task<string> GetInfoAsync(CancellationToken cancellationToken)
        {
            try
            {
                var profile = await _remoteService.GetProfileAsync(cancellationToken).ConfigureAwait(false);

                return string.Concat(profile.FirstName, " ", profile.LastName, " ", profile.Email);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }
}

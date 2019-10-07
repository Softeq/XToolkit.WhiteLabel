// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Threading.Tasks;
using Android.OS;
using Plugin.Permissions;
using Softeq.XToolkit.Common.Files;
using Softeq.XToolkit.Permissions;

namespace Softeq.XToolkit.WhiteLabel.Droid.Providers
{
    public class ExternalStorageProvider : IFilesProvider
    {
        private readonly IPermissionsManager _permissionsManager;
        private readonly string _rootFolderPath;
        private readonly BaseFileProvider _storageProvider;

        public ExternalStorageProvider(IPermissionsManager permissionsManager)
        {
            _rootFolderPath = Environment.ExternalStorageDirectory.Path;
            _storageProvider = new BaseFileProvider();
            _permissionsManager = permissionsManager;
        }

        public async Task ClearFolderAsync(string path)
        {
            await CheckPermission().ConfigureAwait(false);

            await _storageProvider.ClearFolderAsync(Path.Combine(_rootFolderPath, path)).ConfigureAwait(false);
        }

        public async Task<string> CopyFileFromAsync(string path, string newPath)
        {
            await CheckPermission().ConfigureAwait(false);

            return await _storageProvider.CopyFileFromAsync(
                Path.Combine(_rootFolderPath, path),
                Path.Combine(_rootFolderPath, newPath)).ConfigureAwait(false);
        }

        public async Task<bool> ExistsAsync(string path)
        {
            await CheckPermission().ConfigureAwait(false);

            return await _storageProvider.ExistsAsync(Path.Combine(_rootFolderPath, path)).ConfigureAwait(false);
        }

        public async Task<Stream> GetFileContentAsync(string path)
        {
            await CheckPermission().ConfigureAwait(false);

            return await _storageProvider.GetFileContentAsync(Path.Combine(_rootFolderPath, path)).ConfigureAwait(false);
        }

        public async Task<Stream> OpenStreamForWriteAsync(string path)
        {
            await CheckPermission().ConfigureAwait(false);

            return await _storageProvider.OpenStreamForWriteAsync(Path.Combine(_rootFolderPath, path));
        }

        public async Task RemoveAsync(string path)
        {
            await CheckPermission().ConfigureAwait(false);

            await _storageProvider.RemoveAsync(Path.Combine(_rootFolderPath, path)).ConfigureAwait(false);
        }

        public async Task<string> WriteStreamAsync(string path, Stream stream)
        {
            await _storageProvider.WriteStreamAsync(Path.Combine(_rootFolderPath, path), stream).ConfigureAwait(false);

            return path;
        }

        private async Task CheckPermission()
        {
            var status = await _permissionsManager.CheckWithRequestAsync<StoragePermission>().ConfigureAwait(false);
            if (status != PermissionStatus.Granted)
            {
                throw new PermissionNotGrantedException();
            }
        }
    }
}

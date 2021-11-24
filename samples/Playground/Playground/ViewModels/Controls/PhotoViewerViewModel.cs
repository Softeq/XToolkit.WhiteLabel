// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Essentials.FullScreenImage;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Controls
{
    public class PhotoViewerViewModel : ViewModelBase
    {
        private readonly IDialogsService _dialogsService;

        public PhotoViewerViewModel(
            IDialogsService dialogsService)
        {
            _dialogsService = dialogsService;

            OpenCommand = new AsyncCommand(OpenAsync);
        }

        public IAsyncCommand OpenCommand { get; }

        private async Task OpenAsync()
        {
            await _dialogsService
                .For<FullScreenImageViewModel>()
                .WithParam(x => x.ImageUrl, "https://picsum.photos/500/500")
                .Navigate();
        }
    }
}

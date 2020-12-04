// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Playground.RemoteData.Test.Dtos
{
    public class PhotoResponse
    {
        public int AlbumId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common;

namespace Playground.Models
{
    public class ItemModel
    {
        public ItemModel(string title, string description, string iconUrl)
        {
            Title = title;
            Description = description;
            IconUrl = iconUrl;
        }

        public string Title { get; }
        public string Description { get; }
        public string IconUrl { get; }
    }

    public class ItemViewModel : ObservableObject
    {
        private string _title = string.Empty;
        private string _iconUrl = string.Empty;

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public string IconUrl
        {
            get => _iconUrl;
            set => Set(ref _iconUrl, value);
        }
    }
}

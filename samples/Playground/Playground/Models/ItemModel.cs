// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Playground.Models
{
    public class ItemModel
    {
        public string Title { get; }
        public string Description { get; }
        public string IconUrl { get; }

        public ItemModel(string title, string description, string iconUrl)
        {
            Title = title;
            Description = description;
            IconUrl = iconUrl;
        }
    }
}

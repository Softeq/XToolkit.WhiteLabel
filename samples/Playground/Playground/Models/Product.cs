// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Playground.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public double Price { get; set; }
    }
}

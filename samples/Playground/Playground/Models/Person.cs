// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Playground.Models
{
    public class Person
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}
// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;

namespace Softeq.XToolkit.WhiteLabel.Helpers
{
    public static class AvatarPlaceholderBuilder
    {
        public static (string Text, string Color) Build(string name, string[] colors = null)
        {
            if (colors == null)
            {
                colors = new[]
                {
                    "#1abc9c",
                    "#2ecc71",
                    "#3498db",
                    "#9b59b6",
                    "#34495e",
                    "#16a085",
                    "#27ae60",
                    "#2980b9",
                    "#8e44ad",
                    "#2c3e50",
                    "#f1c40f",
                    "#e67e22",
                    "#e74c3c",
                    "#95a5a6",
                    "#f39c12",
                    "#d35400",
                    "#c0392b",
                    "#bdc3c7",
                    "#7f8c8d"
                };
            }

            var abbr = GetAbbreviation(name);
            var index = Math.Abs(abbr.GetHashCode()) % Math.Max(1, colors.Length - 1);

            return (abbr, colors[index]);
        }

        private static string GetAbbreviation(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return string.Empty;
            }

            var trimmedData = data.Trim();

            // case for: First Last -> FL
            if (trimmedData.Contains(' '))
            {
                var splitted = trimmedData.Split(' ');
                if (splitted.Length == 2)
                {
                    var firstSymbol = char.ToUpper(splitted[0].FirstOrDefault());
                    var secondSymbol = char.ToUpper(splitted[1].FirstOrDefault());
                    return string.Concat(firstSymbol, secondSymbol);
                }
            }

            // case for: First .* Last -> FL
            var pascalCase = char.ToUpper(trimmedData[0]) + trimmedData.Substring(1);
            var upperCaseOnly = string.Concat(pascalCase.Where(char.IsUpper));
            if (upperCaseOnly.Length > 1 && upperCaseOnly.Length <= 3)
            {
                return upperCaseOnly.ToUpper();
            }

            // case for: fl -> FL
            if (trimmedData.Length <= 3)
            {
                return trimmedData.ToUpper();
            }

            // case for: SuperLongName -> SUP
            return trimmedData.Substring(0, 3).ToUpper();
        }
    }
}

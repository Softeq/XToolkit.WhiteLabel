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
            var index = Math.Abs(abbr.GetHashCode()) % (colors.Length - 1);

            return (abbr, colors[index]);
        }

        private static string GetAbbreviation(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return string.Empty;
            }

            var trimedData = data.Trim();

            if (trimedData.Contains(' '))
            {
                var splited = trimedData.Split(' ');
                return $"{splited[0].ToUpper()[0]}{splited[1].ToUpper()[0]}";
            }

            var pascalCase = trimedData;
            pascalCase = trimedData.ToUpper()[0] + pascalCase.Substring(1);
            var upperCaseOnly = string.Concat(pascalCase.Where(char.IsUpper));

            if (upperCaseOnly.Length > 1 && upperCaseOnly.Length <= 3)
            {
                return upperCaseOnly.ToUpper();
            }

            if (trimedData.Length <= 3)
            {
                return trimedData.ToUpper();
            }

            return trimedData.Substring(0, 3).ToUpper();
        }
    }
}
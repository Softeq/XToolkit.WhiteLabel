// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;

namespace Softeq.XToolkit.WhiteLabel.Helpers
{
    public static class AvatarPlaceholderBuilder
    {
        /// <summary>
        ///     Creates an abbreviation from the specified username
        ///     and maps it to the color from the predefined list.
        ///     <para/>
        ///     For more info see:
        ///     <para/>
        ///     <see cref="M:Softeq.XToolkit.WhiteLabel.Helpers.AvatarPlaceholderBuilder.GetColor(System.String)"/>.
        ///     <para/>
        ///     <see cref="M:Softeq.XToolkit.WhiteLabel.Helpers.AvatarPlaceholderBuilder.GetAbbreviation(System.String)"/>.
        /// </summary>
        /// <param name="userName">String containing the username to create abbreviation from.</param>
        /// <returns>
        ///     Tuple containing the created abbreviation and picked color.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="userName"/> cannot be <see langword="null"/>.
        /// </exception>
        public static (string Text, string Color) GetAbbreviationAndColor(string userName)
        {
            var abbr = GetAbbreviation(userName);
            var color = GetColor(abbr);

            return (abbr, color);
        }

        /// <summary>
        ///     Creates an abbreviation from the specified username
        ///     and maps it to the color from the specified list.
        ///     <para/>
        ///     For more info see:
        ///     <para/>
        ///     <see cref="M:Softeq.XToolkit.WhiteLabel.Helpers.AvatarPlaceholderBuilder.GetColor(System.String,System.String[])"/>.
        ///     <para/>
        ///     <see cref="M:Softeq.XToolkit.WhiteLabel.Helpers.AvatarPlaceholderBuilder.GetAbbreviation(System.String)"/>.
        /// </summary>
        /// <param name="userName">String containing the username to create abbreviation from.</param>
        /// <param name="colors">List of colors (normally in HEX format) to use.</param>
        /// <returns>
        ///     Tuple containing the created abbreviation and picked color.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="userName"/> and <paramref name="colors"/> cannot be <see langword="null"/>.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="colors"/> cannot be empty.
        /// </exception>
        public static (string Text, string Color) GetAbbreviationAndColor(string userName, string[] colors)
        {
            var abbr = GetAbbreviation(userName);
            var color = GetColor(abbr, colors);

            return (abbr, color);
        }

        /// <summary>
        ///     Maps the specified username to the color from the predefined list.
        ///     The mapping relationship used in this method is many-to-one,
        ///     so that one color can be mapped to many different usernames.
        /// </summary>
        /// <param name="userName">String containing the username to pick color for.</param>
        /// <returns>
        ///     String containing the picked color.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="userName"/> cannot be <see langword="null"/>.
        /// </exception>
        public static string GetColor(string userName)
        {
            var colors = new[]
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
            return GetColor(userName, colors);
        }

        /// <summary>
        ///     Maps the specified username to the color from the specified list.
        ///     The mapping relationship used in this method is many-to-one,
        ///     so that one color can be mapped to many different usernames.
        /// </summary>
        /// <param name="userName">String containing the username to pick color for.</param>
        /// <param name="colors">List of colors (normally in HEX format) to use.</param>
        /// <returns>
        ///     String containing the picked color.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="userName"/> and <paramref name="colors"/> cannot be <see langword="null"/>.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="colors"/> cannot be empty.
        /// </exception>
        public static string GetColor(string userName, string[] colors)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (colors == null)
            {
                throw new ArgumentNullException(nameof(colors));
            }

            if (colors.Length == 0)
            {
                throw new ArgumentException("Colors array cannot be empty", nameof(colors));
            }

            var index = Math.Abs(userName.GetHashCode()) % Math.Max(1, colors.Length - 1);
            return colors[index];
        }

        /// <summary>
        ///     Creates an abbreviation from the specified username.
        ///     Abbreviation will contain no more than 3 uppercase letters.
        /// </summary>
        /// <param name="userName">String containing the username to create abbreviation from.</param>
        /// <returns>
        ///     String containing the abbreviation
        ///     or empty string if the <paramref name="userName"/> string is empty or contains only whitespace characters.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="userName"/> cannot be <see langword="null"/>.
        /// </exception>
        public static string GetAbbreviation(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                return string.Empty;
            }

            var trimmedData = userName.Trim();

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

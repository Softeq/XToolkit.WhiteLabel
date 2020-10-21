// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Helpers.AvatarPlaceholderBuilderTests
{
    internal static class AvatarPlaceholderBuilderTestsDataProvider
    {
        public static TheoryData<string[]> ColorsData
           => new TheoryData<string[]>
           {
               { null },
               { new string[] { } },
               { new string[] { "color1" } },
               { new string[] { "color1", "color2", "color3" } },
           };

        public static TheoryData<string> NoLettersNameData
          => new TheoryData<string>
          {
               { string.Empty },
               { " " },
               { "   " },
               { "1 2 3 4 5" }
          };

        public static TheoryData<string, string> LettersNameData
          => new TheoryData<string, string>
          {
              { "John Doe", "JD" },
              { "   John      Doe   ", "JD" },
              { "john lowercase doe", "JD" },
              { "John Sam Doe", "JD" },
              { "John S Doe", "JD" },
              { "John S. Doe", "JD" },
              { "Matt S. Doe, Jr.", "MD" },
              { "Matt S Doe Sr.", "MD" },
              { "John Doe III", "JD" },
              { "John Doe, III", "JD" },
              { "John X", "JX" },
              { "X Doe", "XD" },
              { "X A Doe", "XD" },
              { "John", "J" },
              { "John Sr.", "J" },
              { "John O'Doe", "JO" },
              { "John McDoe", "JM" },
              { "John \"Nick\" Doe", "JD" },
              { "1John 2Doe", "JD" },
              { "I4U doe589I", "ID" },
              { "존 도우", "존도" },
              { "abracadabra", "A" },
              { "#abracadabra", "A" },
              { "a c d e f g", "AG" },
              { "John not short name doe", "JD" },
              { "John \"not short name\" Doe", "JD" }
          };

        public static TheoryData<string> NonNullNameData
          => new TheoryData<string>
          {
              { string.Empty },
              { " " },
              { "   " },
              { "a" },
              { "Vasily Pupkin" },
              { " Anatoly" },
              { "anatoly " },
              { "  Igor " },
              { "  Ig or " },
              { "John O'Connor" },
              { "Erih Maria Remark" },
              { "SaiNt PeTer garcia Monica 3" }
          };
    }
}

// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Helpers.AvatarPlaceholderBuilderTests
{
    internal static class AvatarPlaceholderBuilderDataProvider
    {
        public static TheoryData<string[]> ColorsData
           => new TheoryData<string[]>
           {
               { null },
               { new string[] { } },
               { new string[] { "color1" } },
               { new string[] { "color1", "color2", "color3" } },
           };

        public static TheoryData<string> EmptyOrWhiteSpaceNameData
          => new TheoryData<string>
          {
               { string.Empty },
               { " " },
               { "   " }
          };

        public static TheoryData<string> NonEmptyNameData
          => new TheoryData<string>
          {
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

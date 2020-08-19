// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Diagnostics;

namespace Softeq.XToolkit.Bindings.Droid.Bindable
{
    [DebuggerDisplay("Type={Type}, Section={SectionIndex}, Item={ItemIndex}")]
    public class FlatItem
    {
        private FlatItem(ItemType type, int sectionIndex, int itemIndex)
        {
            Type = type;
            SectionIndex = sectionIndex;
            ItemIndex = itemIndex;
        }

        public ItemType Type { get; }
        public int SectionIndex { get; }
        public int ItemIndex { get; }

        public static FlatItem CreateForHeader()
        {
            return new FlatItem(ItemType.Header, -1, -1);
        }

        public static FlatItem CreateForFooter()
        {
            return new FlatItem(ItemType.Footer, -1, -1);
        }

        public static FlatItem CreateForSectionHeader(int sectionIndex)
        {
            return new FlatItem(ItemType.SectionHeader, sectionIndex, -1);
        }

        public static FlatItem CreateForSectionFooter(int sectionIndex)
        {
            return new FlatItem(ItemType.SectionFooter, sectionIndex, -1);
        }

        public static FlatItem CreateForItem(int sectionIndex, int itemIndex)
        {
            return new FlatItem(ItemType.Item, sectionIndex, itemIndex);
        }
    }
}

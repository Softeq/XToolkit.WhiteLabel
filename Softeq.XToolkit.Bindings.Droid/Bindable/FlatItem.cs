// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Diagnostics;

namespace Softeq.XToolkit.Bindings.Droid.Bindable
{
    [DebuggerDisplay("Type={Type}, Section={SectionIndex}, Item={ItemIndex}")]
    internal class FlatItem
    {
        private FlatItem(ItemType type, int sectionIndex, int itemIndex)
        {
            Type = type;
            SectionIndex = sectionIndex;
            ItemIndex = itemIndex;
        }

        internal ItemType Type { get; }
        internal int SectionIndex { get; }
        internal int ItemIndex { get; }

        internal static FlatItem CreateForHeader()
        {
            return new FlatItem(ItemType.Header, -1, -1);
        }

        internal static FlatItem CreateForFooter()
        {
            return new FlatItem(ItemType.Footer, -1, -1);
        }

        internal static FlatItem CreateForSectionHeader(int sectionIndex)
        {
            return new FlatItem(ItemType.SectionHeader, sectionIndex, -1);
        }

        internal static FlatItem CreateForSectionFooter(int sectionIndex)
        {
            return new FlatItem(ItemType.SectionFooter, sectionIndex, -1);
        }

        internal static FlatItem CreateForItem(int sectionIndex, int itemIndex)
        {
            return new FlatItem(ItemType.Item, sectionIndex, itemIndex);
        }
    }
}

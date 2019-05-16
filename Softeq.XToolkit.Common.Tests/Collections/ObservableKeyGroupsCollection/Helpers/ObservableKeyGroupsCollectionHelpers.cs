// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.Common.Collections;

namespace Softeq.XToolkit.Tests.Core.Common.Collections.ObservableKeyGroupsCollection.Helpers
{
    public static class ObservableKeyGroupsCollectionHelpers
    {
        public static ObservableKeyGroupsCollection<DateTimeOffset, TestItemModel> CreateSortedGroupCollection(
            List<TestItemModel> items)
        {
            var list = new ObservableKeyGroupsCollection<DateTimeOffset, TestItemModel>(
                message => message.DateTime.Date,
                (x, y) => x.CompareTo(y),
                (x, y) => x.DateTime.CompareTo(y.DateTime));

            list.AddRangeToGroupsSorted(items);

            return list;
        }

        public static List<TestItemModel> CreateRandList(int count, int groupsCount, string text)
        {
            var tempList = new List<TestItemModel>();
            var random = new Random();

            for (var i = 0; i < count; i++)
            {
                var date = DateTime.Now.AddDays(-(i % random.Next(1, groupsCount)));
                tempList.Add(new TestItemModel
                {
                    DateTime = date,
                    Body = $"{text} #{i}"
                });
            }

            return tempList;
        }
    }

    public class TestItemModel
    {
        public DateTimeOffset DateTime { get; set; }
        public string Body { get; set; }

        public bool Equals(TestItemModel other)
        {
            if (other is null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Body == other.Body && DateTime == other.DateTime;
        }
    }

    public class MessageComparer : IEqualityComparer<TestItemModel>
    {
        public bool Equals(TestItemModel x, TestItemModel y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(TestItemModel obj)
        {
            return obj.GetHashCode();
        }
    }
}

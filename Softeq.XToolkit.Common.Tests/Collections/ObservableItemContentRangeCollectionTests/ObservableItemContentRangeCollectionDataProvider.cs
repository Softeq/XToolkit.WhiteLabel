﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableItemContentRangeCollectionTests
{
    internal static class ObservableItemContentRangeCollectionDataProvider
    {
        public static TheoryData<IEnumerable<ObservableObject>> Data
           => new TheoryData<IEnumerable<ObservableObject>>
           {
                { null },
                { new List<ObservableObject>() },
                { new List<ObservableObject>() { new ObservableObject(), new ObservableObject(), new ObservableObject() } },
           };
    }
}

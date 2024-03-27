// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xunit;
using CollectionHelper = Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest.ObservableKeyGroupsCollectionHelper;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest;

public class ObservableKeyGroupsCollectionTestsCountChanged
{
    [Fact]
    public void AddGroups_KeysOnlyForbidEmptyGroupCorrectKeys_RaisesPropertyChangedForCount()
    {
        var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
        var pairs = CollectionHelper.PairNotContainedKeyWithItems;

        Assert.PropertyChanged(collection, nameof(collection.Count), () =>
        {
            collection.AddGroups(pairs);
        });
    }

    [Fact]
    public void InsertGroups_KeysOnlyForbidEmptyGroupCorrectKeys_RaisesPropertyChangedForCount()
    {
        var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
        var pairs = CollectionHelper.PairNotContainedKeyWithItems;

        Assert.PropertyChanged(collection, nameof(collection.Count), () =>
        {
            collection.InsertGroups(0, pairs);
        });
    }

    [Fact]
    public void ReplaceAllGroups_KeysOnlyAllowEmptyGroupCorrectKeys_RaisesPropertyChangedForCount()
    {
        var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
        var keys = CollectionHelper.KeysEmpty;

        Assert.PropertyChanged(collection, nameof(collection.Count), () =>
        {
            collection.ReplaceAllGroups(keys);
        });
    }

    [Fact]
    public void ReplaceAllGroups_KeysOnlyAllowEmptyGroupEmptyCollection_RaisesPropertyChangedForCount()
    {
        var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
        var pairs = CollectionHelper.PairsEmpty;

        Assert.PropertyChanged(collection, nameof(collection.Count), () =>
        {
            collection.ReplaceAllGroups(pairs);
        });
    }

    [Fact]
    public void RemoveGroups_KeysOnlyForbidEmptyGroupContainsKey_RaisesPropertyChangedForCount()
    {
        var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();

        Assert.PropertyChanged(collection, nameof(collection.Count), () =>
        {
            collection.RemoveGroups(CollectionHelper.KeysOneFill);
        });
    }

    [Fact]
    public void Clear_FilledCollection_RaisesPropertyChangedForCount()
    {
        var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();

        Assert.PropertyChanged(collection, nameof(collection.Count), () =>
        {
            collection.Clear();
        });
    }
}

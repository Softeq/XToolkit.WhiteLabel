// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using Foundation;
using Playground.iOS.Views.Collections;
using Playground.ViewModels.Collections;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers.Collections
{
    /// <summary>
    ///     Was ported from Swift:
    ///     https://github.com/TikhonovAlexander/Collection-View-Layout-iOS13/blob/master/Collection-View-Layout-iOS13/View%20Controllers/AdaptiveSectionsViewController.swift
    /// </summary>
    public partial class CompositionalLayoutPageViewController : ViewControllerBase<CompositionalLayoutPageViewModel>
    {
        private UICollectionView _collectionView;

        public CompositionalLayoutPageViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.Title = "Adaptive Sections";

            ConfigureHierarchy();
            ConfigureDataSource();
        }

        private void ConfigureHierarchy()
        {
            _collectionView = new UICollectionView(View.Bounds, CreateLayout());
            _collectionView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            _collectionView.BackgroundColor = UIColor.SystemBackgroundColor;
            _collectionView.RegisterNibForCell(DummyCell.Nib, DummyCell.Key);
            View.AddSubview(_collectionView);
        }

        private UICollectionViewLayout CreateLayout()
        {
            var layout = new UICollectionViewCompositionalLayout((sectionIndex, layoutEnvironment) =>
            {
                var sectionKind = EnumExtensions.FindByValue<Section>((byte) sectionIndex);
                var columns = sectionKind.ColumnCount(layoutEnvironment.Container.EffectiveContentSize.Width);

                var itemSize = NSCollectionLayoutSize.Create(
                    width: NSCollectionLayoutDimension.CreateFractionalWidth(1.0f),
                    height: NSCollectionLayoutDimension.CreateFractionalHeight(1.0f));
                var item = NSCollectionLayoutItem.Create(itemSize);
                item.ContentInsets = new NSDirectionalEdgeInsets(top: 2, leading: 2, bottom: 2, trailing: 2);

                var groupHeight = layoutEnvironment.TraitCollection.VerticalSizeClass == UIUserInterfaceSizeClass.Compact
                    ? NSCollectionLayoutDimension.CreateAbsolute(44)
                    : NSCollectionLayoutDimension.CreateFractionalWidth(0.2f);

                var groupSize = NSCollectionLayoutSize.Create(
                    width: NSCollectionLayoutDimension.CreateFractionalWidth(1.0f),
                    height: groupHeight);
                var group = NSCollectionLayoutGroup.CreateHorizontal(layoutSize: groupSize, subitem: item, count: columns);

                var section = NSCollectionLayoutSection.Create(group);
                section.ContentInsets = new NSDirectionalEdgeInsets(top: 20, leading: 20, bottom: 20, trailing: 20);
                return section;
            });
            return layout;
        }

        private void ConfigureDataSource()
        {
            var dataSource = new UICollectionViewDiffableDataSource<NS<Section>, NS<int>>(
                collectionView: _collectionView,
                cellProvider: (collectionView, indexPath, identifier) =>
                {
                    var cell = (DummyCell) collectionView.DequeueReusableCell(DummyCell.Key, indexPath);

                    cell.Configure(((NS<int>) identifier).Value.ToString());

                    return cell;
                });

            var itemsPerSection = 6;
            var snapshot = new NSDiffableDataSourceSnapshot<NS<Section>, NS<int>>();

            EnumExtensions.Apply<Section>(section =>
            {
                var ns = new NS<Section>(section);
                snapshot.AppendSections(new[] { ns });
                var itemOffset = ((int) ns.Value) * itemsPerSection;
                var itemUpperbound = itemOffset + itemsPerSection;
                var items = Enumerable.Range(itemOffset, itemUpperbound).Select(x => new NS<int>(x)).ToArray();
                snapshot.AppendItems(items);
            });
            dataSource.ApplySnapshot(snapshot, animatingDifferences: false);
        }
    }

    internal enum Section
    {
        List = 0,
        Grid3 = 1,
        Grid5 = 2
    }

    internal static class SectionExtensions
    {
        internal static int ColumnCount(this Section self, nfloat width)
        {
            var wideMode = width > 800;
            return self switch
            {
                Section.List => wideMode ? 2 : 1,
                Section.Grid3 => wideMode ? 6 : 3,
                Section.Grid5 => wideMode ? 10 : 5,
                _ => throw new NotImplementedException()
            };
        }
    }

    // YP: Wrap value to use as NSObject
    internal class NS<T> : NSObject
    {
        public NS(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }
}

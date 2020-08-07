// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using Playground.iOS.Views.Collections;
using Softeq.XToolkit.Common.Extensions;
using UIKit;
using static Playground.iOS.ViewControllers.Collections.CompositionalLayout.NSUtils;
using static UIKit.NSCollectionLayoutDimension;
using Section = Playground.iOS.ViewControllers.Collections.CompositionalLayout.AdaptiveSectionsViewController.Section;

namespace Playground.iOS.ViewControllers.Collections.CompositionalLayout
{
    // Original sources on Swift:
    // https://github.com/TikhonovAlexander/Collection-View-Layout-iOS13/blob/master/Collection-View-Layout-iOS13/View%20Controllers/AdaptiveSectionsViewController.swift
    public class AdaptiveSectionsViewController : UIViewController
    {
        private UICollectionView? _collectionView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

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
                    width: CreateFractionalWidth(1.0f),
                    height: CreateFractionalHeight(1.0f));
                var item = NSCollectionLayoutItem.Create(itemSize);
                item.ContentInsets = new NSDirectionalEdgeInsets(top: 2, leading: 2, bottom: 2, trailing: 2);

                var groupHeight = layoutEnvironment.TraitCollection.VerticalSizeClass == UIUserInterfaceSizeClass.Compact
                    ? CreateAbsolute(44)
                    : CreateFractionalWidth(0.2f);

                var groupSize = NSCollectionLayoutSize.Create(
                    width: CreateFractionalWidth(1.0f),
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

            // initial data
            const int ItemsPerSection = 6;
            var snapshot = new NSDiffableDataSourceSnapshot<NS<Section>, NS<int>>();

            EnumExtensions.Apply<Section>(section =>
            {
                var ns = new NS<Section>(section);
                snapshot.AppendSections(new[] { ns });
                var itemOffset = ((int) ns.Value) * ItemsPerSection;
                var itemUpperbound = itemOffset + ItemsPerSection;
                var items = Enumerable.Range(itemOffset, itemUpperbound).Select(x => new NS<int>(x)).ToArray();
                snapshot.AppendItems(items);
            });
            dataSource.ApplySnapshot(snapshot, animatingDifferences: false);
        }

        internal enum Section
        {
            List = 0,
            Grid3 = 1,
            Grid5 = 2
        }
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
}

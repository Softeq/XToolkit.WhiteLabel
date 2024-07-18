// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using Playground.iOS.Views.Collections;
using UIKit;
using static Playground.iOS.ViewControllers.Collections.CompositionalLayout.NSUtils;
using static UIKit.NSCollectionLayoutDimension;

#pragma warning disable SA1005

namespace Playground.iOS.ViewControllers.Collections.CompositionalLayout
{
    // Original sources on Swift:
    // https://github.com/TikhonovAlexander/Collection-View-Layout-iOS13/blob/master/Collection-View-Layout-iOS13/View%20Controllers/NestedGroupsViewController.swift
    public class NestedGroupsViewController : UIViewController
    {
        private UICollectionView _collectionView = null!;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ConfigureHierarchy();
            ConfigureDataSource();
        }

        private void ConfigureHierarchy()
        {
            _collectionView = new UICollectionView(View!.Bounds, CreateLayout());
            _collectionView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            _collectionView.BackgroundColor = UIColor.SystemBackground;
            _collectionView.RegisterNibForCell(DummyCell.Nib, DummyCell.Key);
            View.AddSubview(_collectionView);
        }

        //   +---------------------------------------------------+
        //   | +-----------------------------------------------+ |
        //   | |                                               | |
        //   | |                                               | |
        //   | |                       0                       | |  <- topItem
        //   | |                                               | |
        //   | |                                               | |
        //   | +-----------------------------------------------+ |
        //   | +---------------------------------+ +-----------+ |
        //   | |                                 | |           | |
        //   | |                                 | |           | |
        //   | |                                 | |     2     | |  <- trailingItem
        //   | |                                 | |           | |
        //   | |                                 | |           | |
        //   | |                                 | +-----------+ |
        //   | |               1                 |               |  <- bottomNestedGroup
        //   | |                                 | +-----------+ |
        //   | |                                 | |           | |
        //   | |                                 | |           | |
        //   | |                                 | |     3     | |  <- trailingItem
        //   | |                                 | |           | |
        //   | |                                 | |           | |
        //   | +---------------------------------+ +-----------+ |
        //   +---------------------------------------------------+
        //                leadingItem              trailingGroup
        private UICollectionViewLayout CreateLayout()
        {
            var layout = new UICollectionViewCompositionalLayout((sectionIndex, layoutEnvironment) =>
            {
                var leadingItem = NSCollectionLayoutItem.Create(
                    layoutSize: NSCollectionLayoutSize.Create(
                        width: CreateFractionalWidth(0.7f),
                        height: CreateFractionalHeight(1.0f)));
                leadingItem.ContentInsets = new NSDirectionalEdgeInsets(top: 10, leading: 10, bottom: 10, trailing: 10);

                var trailingItem = NSCollectionLayoutItem.Create(
                    layoutSize: NSCollectionLayoutSize.Create(
                        width: CreateFractionalWidth(1.0f),
                        height: CreateFractionalHeight(0.3f)));
                trailingItem.ContentInsets = new NSDirectionalEdgeInsets(top: 10, leading: 10, bottom: 10, trailing: 10);

                var trailingGroup = NSCollectionLayoutGroup.CreateVertical(
                    layoutSize: NSCollectionLayoutSize.Create(
                        width: CreateFractionalWidth(0.3f),
                        height: CreateFractionalHeight(1.0f)),
                    subitem: trailingItem,
                    count: 2);

                var bottomNestedGroup = NSCollectionLayoutGroup.CreateHorizontal(
                    layoutSize: NSCollectionLayoutSize.Create(
                        width: CreateFractionalWidth(1.0f),
                        height: CreateFractionalHeight(0.6f)),
                    subitems: new[] { leadingItem, trailingGroup });

                var topItem = NSCollectionLayoutItem.Create(
                    layoutSize: NSCollectionLayoutSize.Create(
                        width: CreateFractionalWidth(1.0f),
                        height: CreateFractionalHeight(0.3f)));
                topItem.ContentInsets = new NSDirectionalEdgeInsets(top: 10, leading: 10, bottom: 10, trailing: 10);

                var nestedGroup = NSCollectionLayoutGroup.CreateVertical(
                    layoutSize: NSCollectionLayoutSize.Create(
                        width: CreateFractionalWidth(1.0f),
                        height: CreateFractionalHeight(0.4f)),
                    subitems: new[] { topItem, bottomNestedGroup });

                var section = NSCollectionLayoutSection.Create(group: nestedGroup);

                section.OrthogonalScrollingBehavior = UICollectionLayoutSectionOrthogonalScrollingBehavior.Continuous;

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
            var snapshot = new NSDiffableDataSourceSnapshot<NS<Section>, NS<int>>();
            snapshot.AppendSections(new[] { new NS<Section>(Section.Main) });
            snapshot.AppendItems(Enumerable.Range(0, 100).Select(x => new NS<int>(x)).ToArray());
            dataSource.ApplySnapshot(snapshot, animatingDifferences: false);
        }

#pragma warning disable SA1201
        internal enum Section
#pragma warning restore SA1201
        {
            Main
        }
    }
}

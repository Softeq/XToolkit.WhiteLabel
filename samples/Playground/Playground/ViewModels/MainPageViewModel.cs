// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Windows.Input;
using Playground.Models;
using Playground.ViewModels.BottomTabs;
using Playground.ViewModels.Collections;
using Playground.ViewModels.Components;
using Playground.ViewModels.Controls;
using Playground.ViewModels.Dialogs;
using Playground.ViewModels.Frames;
using Playground.ViewModels.Pages;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels
{
    internal enum Category
    {
        Navigation,
        Collections,
        Controls,
        Components
    }

    public class MainPageViewModel : ViewModelBase
    {
        private readonly IPageNavigationService _pageNavigationService;
        private readonly IAppInfoService _appInfoService;

        public MainPageViewModel(
            IPageNavigationService pageNavigationService,
            IAppInfoService appInfoService)
        {
            _pageNavigationService = pageNavigationService;
            _appInfoService = appInfoService;

            Items = new ObservableKeyGroupsCollection<string, CommandAction>();
            GoToEmptyCommand = new RelayCommand(GoToEmpty);
        }

        public string Title => "Main";

        public ObservableKeyGroupsCollection<string, CommandAction> Items { get; }

        public ICommand GoToEmptyCommand { get; }

        public override void OnInitialize()
        {
            base.OnInitialize();

            AddItems();
        }

        private void GoToEmpty()
        {
            _pageNavigationService
                .For<EmptyPageViewModel>()
                .NavigateAsync();
        }

        private void AddItems()
        {
            var actions = new List<(Category Header, CommandAction Item)>
            {
                (Category.Navigation, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<DetailsPageViewModel>()
                            .NavigateAsync();
                    }),
                    "Without parameters")),
                (Category.Navigation, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<DetailsPageViewModel>()
                            .WithParam(x => x.Person, new Person { FirstName = "Ivan", LastName = "Ivanov" })
                            .NavigateAsync();
                    }),
                    "With parameters")),
                (Category.Navigation, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<BottomTabsPageViewModel>()
                            .NavigateAsync();
                    }),
                    "Bottom Tabs")),
                (Category.Navigation, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<DialogsPageViewModel>()
                            .NavigateAsync();
                    }),
                    "Dialogs")),
                (Category.Navigation, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<FramesViewModel>()
                            .NavigateAsync();
                    }),
                    "Frames")),
                (Category.Collections, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<TablePageViewModel>()
                            .NavigateAsync();
                    }),
                    "Observable list")),
                (Category.Collections, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<CollectionPageViewModel>()
                            .NavigateAsync();
                    }),
                    "Observable collection")),
                (Category.Collections, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<GroupedTablePageViewModel>()
                            .NavigateAsync();
                    }),
                    "Observable grouped list")),
                (Category.Collections, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<GroupedCollectionPageViewModel>()
                            .NavigateAsync();
                    }),
                    "Observable grouped collection")),
                (Category.Collections, new CommandAction(
                    new RelayCommand(() =>
                    {
                        if (_appInfoService.Platform == Platform.iOS)
                        {
                            _pageNavigationService
                                .For<CompositionalLayoutPageViewModel>()
                                .NavigateAsync();
                        }
                    }),
                    "Compositional Layout (iOS 13+)")),
                (Category.Controls, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<PhotoBrowserViewModel>()
                            .NavigateAsync();
                    }),
                    "Photo browser")),
                (Category.Controls, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<PhotoViewerViewModel>()
                            .NavigateAsync();
                    }),
                    "Photo viewer")),

                // (Category.Components, new CommandAction(
                //   new RelayCommand(() =>
                //   {
                //       _pageNavigationService.NavigateToViewModel<FilesViewModel>();
                //   }),
                //   "Files")),
                (Category.Components, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<PermissionsPageViewModel>()
                            .NavigateAsync();
                    }),
                    "Permissions")),
                (Category.Components, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<ConnectivityPageViewModel>()
                            .NavigateAsync();
                    }),
                    "Connectivity")),
                (Category.Components, new CommandAction(
                    new RelayCommand(() =>
                    {
                        if (_appInfoService.Platform == Platform.iOS)
                        {
                            _pageNavigationService
                                .For<GesturesPageViewModel>()
                                .NavigateAsync();
                        }
                    }),
                    "Gestures (iOS)"))
            };

            Items.AddItems(actions, x => x.Header.ToString(), x => x.Item);
        }
    }
}

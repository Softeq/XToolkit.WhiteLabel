﻿// Developed by Softeq Development Corporation
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
                .Navigate();
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
                            .Navigate();
                    }),
                    "Without parameters")),
                (Category.Navigation, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<DetailsPageViewModel>()
                            .WithParam(x => x.Person, new Person { FirstName = "Ivan", LastName = "Ivanov" })
                            .Navigate();
                    }),
                    "With parameters")),
                (Category.Navigation, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<BottomTabsPageViewModel>()
                            .Navigate();
                    }),
                    "Bottom Tabs")),
                (Category.Navigation, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<DialogsPageViewModel>()
                            .Navigate();
                    }),
                    "Dialogs")),
                (Category.Navigation, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<FramesViewModel>()
                            .Navigate();
                    }),
                    "Frames")),
                (Category.Collections, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<TablePageViewModel>()
                            .Navigate();
                    }),
                    "Observable list")),
                (Category.Collections, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<CollectionPageViewModel>()
                            .Navigate();
                    }),
                    "Observable collection")),
                (Category.Collections, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<GroupedTablePageViewModel>()
                            .Navigate();
                    }),
                    "Observable grouped list")),
                (Category.Collections, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<GroupedCollectionPageViewModel>()
                            .Navigate();
                    }),
                    "Observable grouped collection")),
                (Category.Collections, new CommandAction(
                    new RelayCommand(() =>
                    {
                        if (_appInfoService.Platform == Platform.iOS)
                        {
                            _pageNavigationService
                                .For<CompositionalLayoutPageViewModel>()
                                .Navigate();
                        }
                    }),
                    "Compositional Layout (iOS 13+)")),
                (Category.Controls, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<PhotoBrowserViewModel>()
                            .Navigate();
                    }),
                    "Photo browser")),
                (Category.Controls, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<PhotoViewerViewModel>()
                            .Navigate();
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
                            .Navigate();
                    }),
                    "Permissions")),
                (Category.Components, new CommandAction(
                    new RelayCommand(() =>
                    {
                        _pageNavigationService
                            .For<ConnectivityPageViewModel>()
                            .Navigate();
                    }),
                    "Connectivity")),
                (Category.Components, new CommandAction(
                    new RelayCommand(() =>
                    {
                        if (_appInfoService.Platform == Platform.iOS)
                        {
                            _pageNavigationService
                                .For<GesturesPageViewModel>()
                                .Navigate();
                        }
                    }),
                    "Gestures (iOS)"))
            };

            Items.AddItems(actions, x => x.Header.ToString(), x => x.Item);
        }
    }
}

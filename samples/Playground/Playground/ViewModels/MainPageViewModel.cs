// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Windows.Input;
using Playground.Models;
using Playground.ViewModels.BottomTabs;
using Playground.ViewModels.Collections;
using Playground.ViewModels.Components;
using Playground.ViewModels.Dialogs;
using Playground.ViewModels.Pages;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IPageNavigationService _pageNavigationService;

        public MainPageViewModel(
            IPageNavigationService pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;
        }

        public string Title => "Main";

        public ObservableKeyGroupsCollection<string, CommandAction> Items { get; } =
            new ObservableKeyGroupsCollection<string, CommandAction>();

        public ICommand GoToEmptyCommand { get; private set; }

        public override void OnInitialize()
        {
            base.OnInitialize();

            var actions = new List<(Category Header, CommandAction Item)>
            {
                (Category.Navigation,
                    new CommandAction
                    {
                        Title = "Without parameters",
                        Command = new RelayCommand(() =>
                        {
                            _pageNavigationService
                                .For<DetailsPageViewModel>()
                                .Navigate();
                        })
                    }),
                (Category.Navigation,
                    new CommandAction
                    {
                        Title = "With parameters",
                        Command = new RelayCommand(() =>
                        {
                            _pageNavigationService
                                .For<DetailsPageViewModel>()
                                .WithParam(x => x.Person, new Person
                                {
                                    FirstName = "Ivan",
                                    LastName = "Ivanov"
                                })
                                .Navigate();
                        })
                    }),
                (Category.Navigation,
                    new CommandAction
                    {
                        Title = "Bottom Tabs",
                        Command = new RelayCommand(() =>
                        {
                            _pageNavigationService
                                .For<BottomTabsPageViewModel>()
                                .Navigate();
                        })
                    }),
                (Category.Navigation,
                    new CommandAction
                    {
                        Title = "Dialogs",
                        Command = new RelayCommand(() =>
                        {
                            _pageNavigationService
                                .For<DialogsPageViewModel>()
                                .Navigate();
                        })
                    }),
                (Category.Collections,
                    new CommandAction
                    {
                        Title = "Observable list",
                        Command = new RelayCommand(() =>
                        {
                            _pageNavigationService
                                .For<TablePageViewModel>()
                                .Navigate();
                        })
                    }),
                (Category.Collections,
                    new CommandAction
                    {
                        Title = "Observable collection",
                        Command = new RelayCommand(() =>
                        {
                            _pageNavigationService
                                .For<CollectionPageViewModel>()
                                .Navigate();
                        })
                    }),
                (Category.Collections,
                    new CommandAction
                    {
                        Title = "Observable grouped list",
                        Command = new RelayCommand(() =>
                        {
                            _pageNavigationService.NavigateToViewModel<GroupedTablePageViewModel>();
                        })
                    }),
                (Category.Collections,
                    new CommandAction
                    {
                        Title = "Observable grouped collection",
                        Command = new RelayCommand(() =>
                        {
                            _pageNavigationService.NavigateToViewModel<GroupedCollectionPageViewModel>();
                        })
                    }),
                (Category.Controls,
                    new CommandAction
                    {
                        Title = "Photo browser",
                        Command = new RelayCommand(() =>
                        {
                            //_pageNavigationService.NavigateToViewModel<PhotoBrowserViewModel>();
                        })
                    }),
                (Category.Components,
                    new CommandAction
                    {
                        Title = "Files",
                        Command = new RelayCommand(() =>
                        {
                            //_pageNavigationService.NavigateToViewModel<FilesViewModel>();
                        })
                }),
                (Category.Components,
                    new CommandAction
                    {
                        Title = "Permissions",
                        Command = new RelayCommand(() =>
                        {
                            _pageNavigationService
                                .For<PermissionsPageViewModel>()
                                .Navigate();
                        })
                    })
            };

            Items.AddRangeToGroups(actions, x => x.Item, x => x.Header.ToString());

            GoToEmptyCommand = new RelayCommand(GoToEmpty);
        }

        private void GoToEmpty()
        {
            _pageNavigationService
                .For<EmptyPageViewModel>()
                .Navigate();
        }
    }

    internal enum Category
    {
        Navigation,
        Collections,
        Controls,
        Components
    }
}
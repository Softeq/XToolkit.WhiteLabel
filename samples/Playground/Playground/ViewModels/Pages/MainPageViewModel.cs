// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Playground.Models;

namespace Playground.ViewModels.Pages
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IPageNavigationService _pageNavigationService;

        public MainPageViewModel(
            IPageNavigationService pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;
        }

        public ObservableKeyGroupsCollection<string, CommandAction> Items { get; } =
            new ObservableKeyGroupsCollection<string, CommandAction>();

        public override void OnInitialize()
        {
            base.OnInitialize();

            var actions = new List<(string Header, CommandAction Item)>
            {
                ("Navigation",
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
                ("Navigation",
                    new CommandAction
                    {
                        Title = "With parameters",
                        Command = new RelayCommand(() =>
                        {
                            _pageNavigationService
                                .For<DetailsPageViewModel>()
                                .WithParam(x => x.Parameter, new Person
                                {
                                    FirstName = "Ivan",
                                    LastName = "Ivanov"
                                })
                                .Navigate();
                        })
                    }),
                ("Navigation",
                    new CommandAction
                    {
                        Title = "Bottom Tabs",
                        Command = new RelayCommand(() =>
                        {
                            _pageNavigationService
                                .For<ToolbarPageViewModel>()
                                .Navigate();
                        })
                    }),
                ("Collections",
                    new CommandAction
                    {
                        Title = "Observable list",
                        Command = new RelayCommand(() =>
                        {
                            _pageNavigationService
                                .For<CollectionPageViewModel>()
                                .Navigate();
                        })
                    }),
                ("Collections",
                    new CommandAction
                    {
                        Title = "Observable groupped list",
                        Command = new RelayCommand(() =>
                        {
                            //_pageNavigationService.NavigateToViewModel<GrouppedCollectionViewModel>();
                        })
                    }),
                ("Controls",
                    new CommandAction
                    {
                        Title = "Photo browser",
                        Command = new RelayCommand(() =>
                        {
                            //_pageNavigationService.NavigateToViewModel<PhotoBrowserViewModel>();
                        })
                    }),
                ("Components",
                    new CommandAction
                    {
                        Title = "Files",
                        Command = new RelayCommand(() =>
                        {
                        //_pageNavigationService.NavigateToViewModel<FilesViewModel>();
                        })
                }),
                ("Components",
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

            Items.AddRangeToGroups(actions, x => x.Item, x => x.Header);
        }
    }
}
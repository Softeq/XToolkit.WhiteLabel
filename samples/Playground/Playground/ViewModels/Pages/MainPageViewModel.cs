// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using Playground.Models;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

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

            if (Items.Any())
            {
                return;
            }

            var actions = new List<(string Header, CommandAction Item)>
            {
                ("Navigation",
                    new CommandAction
                    {
                        Title = "Without parameter",
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
                        Title = "With parameter",
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
                        Title = "Toolbar page",
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
                            //_pageNavigationService.NavigateToViewModel<CollectionViewModel>();
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
                ("Collections",
                    new CommandAction
                    {
                        Title = "Observable Texture list",
                        Command = new RelayCommand(() =>
                        {
                            //_pageNavigationService.NavigateToViewModel<TextureCollectionViewModel>();
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
                        Title = "Stripe",
                        Command = new RelayCommand(() =>
                        {
                            //_pageNavigationService.NavigateToViewModel<StripeViewModel>();
                        })
                    }),
                ("Components",
                    new CommandAction
                    {
                        Title = "Cached requests",
                        Command = new RelayCommand(() =>
                        {
                            //_pageNavigationService.NavigateToViewModel<CachedRequestsViewModel>();
                        })
                    }),
                ("Components",
                    new CommandAction
                    {
                        Title = "Connectivity",
                        Command = new RelayCommand(() =>
                        {
                            //_pageNavigationService.NavigateToViewModel<ConnectivityViewModel>();
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
                        //_pageNavigationService.NavigateToViewModel<PermissionsViewModel >();
                        })
                    }),
                ("Components",
                    new CommandAction
                    {
                        Title = "Image Picker with cropping",
                        Command = new RelayCommand(() =>
                        {
                        //_pageNavigationService.NavigateToViewModel<ImagePickerViewModel >();
                        })
                    })
            };

            Items.AddRangeToGroups(actions, x => x.Item, x => x.Header);
        }
    }
}
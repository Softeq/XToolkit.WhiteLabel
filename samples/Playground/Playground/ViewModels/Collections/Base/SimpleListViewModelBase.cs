// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Playground.Models;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Collections.Base
{
    public abstract class SimpleListViewModelBase : ViewModelBase
    {
        private readonly IDialogsService _dialogsService;

        public SimpleListViewModelBase(IDialogsService dialogsService)
        {
            _dialogsService = dialogsService;

            Items = GetMovies();

            SelectItemCommand = new AsyncCommand<ItemViewModel>(SelectItem);
        }

        public List<ItemModel> Items { get; }

        public ObservableRangeCollection<ItemViewModel> ItemModels = new ObservableRangeCollection<ItemViewModel>();

        public ICommand<ItemViewModel> SelectItemCommand { get; }

        public override async void OnInitialize()
        {
            base.OnInitialize();

            await Task.Delay(1000);

            ItemModels.AddRange(GenerateItems());

            for (var i = 0; i < 50; i++)
            {
                ItemModels.Apply(x => x.Title += i);

                await Task.Delay(1000);
            }
        }

        private async Task SelectItem(ItemViewModel viewModel)
        {
            await _dialogsService.ShowDialogAsync("Selected", viewModel.Title, "OK");
        }

        public List<ItemModel> GetMovies()
        {
            return new List<ItemModel>
            {
                new ItemModel("Doctor Strange 2","Plot unknown.","https://image.tmdb.org/t/p/w92/lTXMtlN9tMrdXalN5DUc1Hiz4Uc.jpg"),
                new ItemModel("Gambit","Channing Tatum stars as Remy LeBeau, aka Gambit, the Cajun Mutant with a knack for cards.","https://image.tmdb.org/t/p/w92/bk8HLj657qGW4LNPBAGGunohoMb.jpg"),
                new ItemModel("Grudge","A house is cursed by a vengeful ghost that dooms those who enter it with a violent death.","https://image.tmdb.org/t/p/w92/praED7kjub5vc94W1A5ZysCRVY7.jpg"),
                new ItemModel("World War Z 2","The plot is currently unknown.","https://image.tmdb.org/t/p/w92/ctjpymgEDwRTQ4DrzUEFbcwwGKu.jpg"),
                new ItemModel("Jumanji 3","The plot is currently unknown.","https://image.tmdb.org/t/p/w92/kjw7FCKXZGK9S1dKPi4EKVIEmj7.jpg"),
                new ItemModel("Masters of the Universe","He-Man, the most powerful man in the universe, goes against the evil Skeletor to save the planet Eternia and protect the secrets of Castle Grayskull.","https://showtimesth.s3.amazonaws.com/_s_/2018/12/09679d50bf9f1b076a994d4694eaafe7d7a35dce-thumbnail-185x278-85.jpg?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAJRS5DOPJNJGKH63A%2F20190526%2Fap-southeast-1%2Fs3%2Faws4_request&X-Amz-Date=20190526T201627Z&X-Amz-Expires=432000&X-Amz-SignedHeaders=host&X-Amz-Signature=426d304dc7e6ffed75e1e6448a2713d52e7af89ceebff6a3304af8374ade6877"),
                new ItemModel("Star Wars: The Rise of Skywalker","The next installment in the franchise, and the conclusion of the “Star Wars“ sequel trilogy as well as the “Skywalker Saga“.","https://image.tmdb.org/t/p/w92/wMgSRRDJIoxuEY9SmKeiB6wr0pd.jpg"),
                new ItemModel("The Angry Birds Movie 2","The flightless birds and scheming green pigs take their beef to the next level.","https://image.tmdb.org/t/p/w92/6ifkVBHrfguWtn7bOgSoofB7eBz.jpg"),
                new ItemModel("Frozen 2","Sequel to Disney's FROZEN, currently in development.","https://image.tmdb.org/t/p/w92/yT2OSZOSdVVtE6OXoXu2Ke1APCC.jpg"),
                new ItemModel("Zombieland: Double Tap","Columbus, Tallahasse, Wichita, and Little Rock move to the American heartland as they face off against evolved zombies, fellow survivors, and the growing pains of the snarky makeshift family.","https://image.tmdb.org/t/p/w92/uSA14LS7GTELnYT7NgR3BcfLsWS.jpg"),
                new ItemModel("You Are My Friend","The story of Fred Rogers, the honored host and creator of the popular children's television program, Mister Rogers' Neighborhood(1968). ","https://showtimesth.s3.amazonaws.com/_s_/2018/12/1e288556d7e68bbbd0a1ce5a00ca35f34eff143b-thumbnail-185x278-85.jpg?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAJRS5DOPJNJGKH63A%2F20190526%2Fap-southeast-1%2Fs3%2Faws4_request&X-Amz-Date=20190526T201627Z&X-Amz-Expires=432000&X-Amz-SignedHeaders=host&X-Amz-Signature=ad426b8358b99274e60fe31a2c46857a91cad398afd9529030f7c985dee5ef0a"),
            };
        }

        private IEnumerable<ItemViewModel> GenerateItems()
        {
            return Enumerable.Range(0, 100).Select(i => new ItemViewModel
            {
                Title = $"{i} #- Title",
                IconUrl = $"https://picsum.photos/100/150?random={i}"
            });
        }
    }
}

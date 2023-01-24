// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Playground.ViewModels.BLoC.Flows;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Messenger;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.BLoC
{
    public class EditedMessage
    {
        public EditedMessage(string result)
        {
            Result = result;
        }

        public string Result { get; }
    }

    public class BlocDetailViewModel : ViewModelBase
    {
        public BlocDetailViewModel()
        {
            EditCommand = new AsyncCommand(OnEdit);
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ICommand EditCommand { get; private set; }
        public Action<string> Edit { get; internal set; }

        private async Task OnEdit()
        {
            Messenger.Default.Register<SaveNameMessage>(this, msg =>
            {
                Messenger.Default.Unregister<SaveNameMessage>(this);

                Name = msg.Name;
            });

            Messenger.Default.Send<EditNameMessage>(new EditNameMessage(Name));
        }
    }
}

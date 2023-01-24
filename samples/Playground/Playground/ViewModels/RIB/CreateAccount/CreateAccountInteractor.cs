// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Playground.ViewModels.RIB.Login;
using Playground.ViewModels.RIB.Platform;

namespace Playground.ViewModels.RIB.CreateAccount
{
    public interface ICreateAccountRouting : IViewvableRouter
    {
        void RouteToMain();
    }

    public interface IMainPresentable
    {
        ICreateAccountPresentableListener Listener { get; set; }
    }

    public interface ICreateAccountListener
    {
        void CloseCreateAccountFlow();
        void Register(string name, string password);
    }

    public interface ICreateAccountPresentable
    {
        ICreateAccountPresentableListener? Listener { get; set; }

        void ShowActivityIndicator(bool v);
        void ShowErrorAlert();
    }

    public class CreateAccountInteractor : PresentableInteractor<ICreateAccountPresentable>,
        ICreateAccountInteractable,
        ICreateAccountPresentableListener
    {
        public override void Init(ICreateAccountPresentable presenter)
        {
            base.Init(presenter);

            presenter.Listener = this;
        }

        public ICreateAccountRouting Router { get; set; }
        public ICreateAccountListener Listener { get; set; }

        public void DidTapClose()
        {
            Listener?.CloseCreateAccountFlow();
        }

        public async void DidTapRegister(string name, string password)
        {
            try
            {
                Presenter?.ShowActivityIndicator(true);
                await Task.Delay(2000);
                Listener?.Register(name, password);
            }
            catch
            {
                Presenter?.ShowErrorAlert();
            }
            finally
            {
                Router?.RouteToMain();
            }
        }
    }
}

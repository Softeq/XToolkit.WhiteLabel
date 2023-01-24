// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Playground.ViewModels.RIB.Platform;
using Playground.ViewModels.RIB.Services.Interfaces;

namespace Playground.ViewModels.RIB.Login
{
    public interface ILoginRouting : IViewvableRouter
    {
        void RouteToCreateAccount();
        void DetachCreateAccount();
    }

    public interface ILoginListener
    {
        void DismissLoginFlow();
    }

    public interface ILoginPresentable
    {
        ILoginPresentableListener? Listener { get; set; }
        void ShowActivityIndicator(bool isLoading);
        void ShowErrorAlert();
    }

    public class LoginInteractor : PresentableInteractor<ILoginPresentable>,
        ILoginInteractable,
        ILoginPresentableListener
    {
        private readonly ILoginService _loginService;

        public ILoginRouting Router { get; set; }
        public ILoginListener Listener { get; set; }

        public LoginInteractor(
            ILoginService loginService)
        {
            _loginService = loginService;
        }

        public void Init(
            ILoginPresentable presenter)
        {
            base.Init(presenter);

            presenter.Listener = this;
        }

        public void DidTapCreateAccount()
        {
            Router?.RouteToCreateAccount();
        }

        public async void DidTapLogin(string username, string password)
        {
            try
            {
                Presenter.ShowActivityIndicator(true);
                if (await _loginService.Login(username, password))
                {
                    Listener.DismissLoginFlow();
                }
            }
            catch
            {
                Presenter.ShowErrorAlert();
            }
            finally
            {
                Presenter.ShowActivityIndicator(false);
            }
        }
    }
}

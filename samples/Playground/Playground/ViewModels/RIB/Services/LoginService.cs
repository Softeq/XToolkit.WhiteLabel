// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Playground.ViewModels.RIB.Services.Interfaces;

namespace Playground.ViewModels.RIB.Services
{
    public class LoginService : ILoginService
    {
        public LoginService()
        {
        }

        public async Task<bool> Login(string name, string password)
        {
            await Task.Delay(1500);
            return true;
        }
    }
}

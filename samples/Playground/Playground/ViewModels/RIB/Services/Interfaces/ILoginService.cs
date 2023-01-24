// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Playground.ViewModels.RIB.Services.Interfaces
{
    public interface ILoginService
    {
        Task<bool> Login(string name, string password);
    }
}

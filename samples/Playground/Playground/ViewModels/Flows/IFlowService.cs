// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Playground.ViewModels.Flows
{
    public interface IFlowService
    {
        Task ProcessAsync(IFlowModel flowModel);
    }
}

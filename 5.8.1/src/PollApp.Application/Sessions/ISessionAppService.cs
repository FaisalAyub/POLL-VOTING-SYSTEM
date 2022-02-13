using System.Threading.Tasks;
using Abp.Application.Services;
using PollApp.Sessions.Dto;

namespace PollApp.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}

using System.Threading.Tasks;
using Abp.Application.Services;
using PollApp.Authorization.Accounts.Dto;

namespace PollApp.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}

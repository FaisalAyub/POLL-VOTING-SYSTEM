using Abp.Application.Services;
using PollApp.MultiTenancy.Dto;

namespace PollApp.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}


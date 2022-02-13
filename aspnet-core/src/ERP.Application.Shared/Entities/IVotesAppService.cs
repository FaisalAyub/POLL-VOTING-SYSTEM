using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Entities.Dtos;
using ERP.Dto;

namespace ERP.Entities
{
    public interface IVotesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetVoteForViewDto>> GetAll(GetAllVotesInput input);

        Task<GetVoteForViewDto> GetVoteForView(int id);

		Task<GetVoteForEditOutput> GetVoteForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditVoteDto input);

		Task Delete(EntityDto input);

		
		Task<PagedResultDto<VotePollLookupTableDto>> GetAllPollForLookupTable(GetAllForLookupTableInput input);
		
    }
}
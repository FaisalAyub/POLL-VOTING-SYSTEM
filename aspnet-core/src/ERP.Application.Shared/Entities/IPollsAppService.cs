using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Entities.Dtos;
using ERP.Dto;

namespace ERP.Entities
{
    public interface IPollsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPollForViewDto>> GetAll(GetAllPollsInput input);

        Task<GetPollForViewDto> GetPollForView(int id);

		Task<GetPollForEditOutput> GetPollForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditPollDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPollsToExcel(GetAllPollsForExcelInput input);

		
		Task<PagedResultDto<PollUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}
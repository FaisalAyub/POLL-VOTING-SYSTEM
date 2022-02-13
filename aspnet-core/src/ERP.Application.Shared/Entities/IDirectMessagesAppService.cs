using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Entities.Dtos;
using ERP.Dto;

namespace ERP.Entities
{
    public interface IDirectMessagesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDirectMessageForViewDto>> GetAll(GetAllDirectMessagesInput input);

        Task<GetDirectMessageForViewDto> GetDirectMessageForView(int id);

		Task<GetDirectMessageForEditOutput> GetDirectMessageForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditDirectMessageDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetDirectMessagesToExcel(GetAllDirectMessagesForExcelInput input);

		
		Task<PagedResultDto<DirectMessageUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}
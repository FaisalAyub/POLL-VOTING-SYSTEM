using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Entities.Dtos;
using ERP.Dto;

namespace ERP.Entities
{
    public interface ICommentsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCommentForViewDto>> GetAll(GetAllCommentsInput input);

        Task<GetCommentForViewDto> GetCommentForView(int id);

		Task<GetCommentForEditOutput> GetCommentForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditCommentDto input);

		Task Delete(EntityDto input);

		
		Task<PagedResultDto<CommentPollLookupTableDto>> GetAllPollForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<CommentUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}
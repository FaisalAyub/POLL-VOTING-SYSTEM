

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Entities.Exporting;
using ERP.Entities.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.Entities
{
	[AbpAuthorize(AppPermissions.Pages_Polls)]
    public class PollsAppService : ERPAppServiceBase, IPollsAppService
    {
		 private readonly IRepository<Poll> _pollRepository;
		 private readonly IPollsExcelExporter _pollsExcelExporter;
		 

		  public PollsAppService(IRepository<Poll> pollRepository, IPollsExcelExporter pollsExcelExporter ) 
		  {
			_pollRepository = pollRepository;
			_pollsExcelExporter = pollsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetPollForViewDto>> GetAll(GetAllPollsInput input)
         {
			
			var filteredPolls = _pollRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.Option1.Contains(input.Filter) || e.Option2.Contains(input.Filter) || e.Option3.Contains(input.Filter) || e.Option4.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter),  e => e.Title.ToLower() == input.TitleFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.Option1Filter),  e => e.Option1.ToLower() == input.Option1Filter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.Option2Filter),  e => e.Option2.ToLower() == input.Option2Filter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.Option3Filter),  e => e.Option3.ToLower() == input.Option3Filter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.Option4Filter),  e => e.Option4.ToLower() == input.Option4Filter.ToLower().Trim());

			var pagedAndFilteredPolls = filteredPolls
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var polls = from o in pagedAndFilteredPolls
                         select new GetPollForViewDto() {
							Poll = new PollDto
							{
                                Title = o.Title,
                                Option1 = o.Option1,
                                Option2 = o.Option2,
                                Option3 = o.Option3,
                                Option4 = o.Option4,
                                Id = o.Id,								
							}
						};

            var totalCount = await filteredPolls.CountAsync();

            return new PagedResultDto<GetPollForViewDto>(
                totalCount,
                await polls.ToListAsync()
            );
         }
		 
		 public async Task<GetPollForViewDto> GetPollForView(int id)
         {
            var poll = await _pollRepository.GetAsync(id);

            var output = new GetPollForViewDto { Poll = ObjectMapper.Map<PollDto>(poll) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Polls_Edit)]
		 public async Task<GetPollForEditOutput> GetPollForEdit(EntityDto input)
         {
            var poll = await _pollRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPollForEditOutput {Poll = ObjectMapper.Map<CreateOrEditPollDto>(poll)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPollDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Polls_Create)]
		 private async Task Create(CreateOrEditPollDto input)
         {
            var poll = ObjectMapper.Map<Poll>(input);

			

            await _pollRepository.InsertAsync(poll);
         }

		 [AbpAuthorize(AppPermissions.Pages_Polls_Edit)]
		 private async Task Update(CreateOrEditPollDto input)
         {
            var poll = await _pollRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, poll);
         }

		 [AbpAuthorize(AppPermissions.Pages_Polls_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _pollRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPollsToExcel(GetAllPollsForExcelInput input)
         {
			
			var filteredPolls = _pollRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.Option1.Contains(input.Filter) || e.Option2.Contains(input.Filter) || e.Option3.Contains(input.Filter) || e.Option4.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter),  e => e.Title.ToLower() == input.TitleFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.Option1Filter),  e => e.Option1.ToLower() == input.Option1Filter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.Option2Filter),  e => e.Option2.ToLower() == input.Option2Filter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.Option3Filter),  e => e.Option3.ToLower() == input.Option3Filter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.Option4Filter),  e => e.Option4.ToLower() == input.Option4Filter.ToLower().Trim());

			var query = (from o in filteredPolls
                         select new GetPollForViewDto() { 
							Poll = new PollDto
							{
                                Title = o.Title,
                                Option1 = o.Option1,
                                Option2 = o.Option2,
                                Option3 = o.Option3,
                                Option4 = o.Option4,
                                Id = o.Id
							}
						 });


            var pollListDtos = await query.ToListAsync();

            return _pollsExcelExporter.ExportToFile(pollListDtos);
         }


    }
}
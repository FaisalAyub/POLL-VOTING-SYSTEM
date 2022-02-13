using ERP.Authorization.Users;


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
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public PollsAppService(IRepository<Poll> pollRepository, IPollsExcelExporter pollsExcelExporter , IRepository<User, long> lookup_userRepository) 
		  {
			_pollRepository = pollRepository;
			_pollsExcelExporter = pollsExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		
		  }

		 public async Task<PagedResultDto<GetPollForViewDto>> GetAll(GetAllPollsInput input)
         {
			
			var filteredPolls = _pollRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.Option1.Contains(input.Filter) || e.Option2.Contains(input.Filter) || e.Option3.Contains(input.Filter) || e.Option4.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter),  e => e.Title.ToLower() == input.TitleFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.Option1Filter),  e => e.Option1.ToLower() == input.Option1Filter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.Option2Filter),  e => e.Option2.ToLower() == input.Option2Filter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.Option3Filter),  e => e.Option3.ToLower() == input.Option3Filter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.Option4Filter),  e => e.Option4.ToLower() == input.Option4Filter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name.ToLower() == input.UserNameFilter.ToLower().Trim());

			var pagedAndFilteredPolls = filteredPolls
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var polls = from o in pagedAndFilteredPolls
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetPollForViewDto() {
							Poll = new PollDto
							{
                                Title = o.Title,
                                Option1 = o.Option1,
                                Option2 = o.Option2,
                                Option3 = o.Option3,
                                Option4 = o.Option4,
                                count1 = o.count1,
                                count2 = o.count2,
                                count3 = o.count3,
                                count4 = o.count4,
                                Id = o.Id,
                                UserId=o.UserId,
                                
                            },
                            
                         	UserName = s1 == null ? "" : s1.Name.ToString()
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

		    if (output.Poll.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Poll.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Polls_Edit)]
		 public async Task<GetPollForEditOutput> GetPollForEdit(EntityDto input)
         {
            var poll = await _pollRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPollForEditOutput {Poll = ObjectMapper.Map<CreateOrEditPollDto>(poll)};

		    if (output.Poll.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Poll.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPollDto input)
         {
            input.UserId = Convert.ToInt32(AbpSession.UserId);
            if (input.Id == null){
                input.count1 = 0;
                input.count2 = 0;
                input.count3 = 0;
                input.count4 = 0;
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
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.Option1.Contains(input.Filter) || e.Option2.Contains(input.Filter) || e.Option3.Contains(input.Filter) || e.Option4.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter),  e => e.Title.ToLower() == input.TitleFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.Option1Filter),  e => e.Option1.ToLower() == input.Option1Filter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.Option2Filter),  e => e.Option2.ToLower() == input.Option2Filter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.Option3Filter),  e => e.Option3.ToLower() == input.Option3Filter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.Option4Filter),  e => e.Option4.ToLower() == input.Option4Filter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name.ToLower() == input.UserNameFilter.ToLower().Trim());

			var query = (from o in filteredPolls
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetPollForViewDto() { 
							Poll = new PollDto
							{
                                Title = o.Title,
                                Option1 = o.Option1,
                                Option2 = o.Option2,
                                Option3 = o.Option3,
                                Option4 = o.Option4,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString()
						 });


            var pollListDtos = await query.ToListAsync();

            return _pollsExcelExporter.ExportToFile(pollListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Polls)]
         public async Task<PagedResultDto<PollUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<PollUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new PollUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<PollUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}
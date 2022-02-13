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
	[AbpAuthorize(AppPermissions.Pages_DirectMessages)]
    public class DirectMessagesAppService : ERPAppServiceBase, IDirectMessagesAppService
    {
		 private readonly IRepository<DirectMessage> _directMessageRepository;
		 private readonly IDirectMessagesExcelExporter _directMessagesExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public DirectMessagesAppService(IRepository<DirectMessage> directMessageRepository, IDirectMessagesExcelExporter directMessagesExcelExporter , IRepository<User, long> lookup_userRepository) 
		  {
			_directMessageRepository = directMessageRepository;
			_directMessagesExcelExporter = directMessagesExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		
		  }

		 public async Task<PagedResultDto<GetDirectMessageForViewDto>> GetAll(GetAllDirectMessagesInput input)
         {
			
			var filteredDirectMessages = _directMessageRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.text.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.textFilter),  e => e.text.ToLower() == input.textFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name.ToLower() == input.UserNameFilter.ToLower().Trim());

			var pagedAndFilteredDirectMessages = filteredDirectMessages
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var directMessages = from o in pagedAndFilteredDirectMessages
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetDirectMessageForViewDto() {
							DirectMessage = new DirectMessageDto
							{
                                text = o.text,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredDirectMessages.CountAsync();

            return new PagedResultDto<GetDirectMessageForViewDto>(
                totalCount,
                await directMessages.ToListAsync()
            );
         }
		 
		 public async Task<GetDirectMessageForViewDto> GetDirectMessageForView(int id)
         {
            var directMessage = await _directMessageRepository.GetAsync(id);

            var output = new GetDirectMessageForViewDto { DirectMessage = ObjectMapper.Map<DirectMessageDto>(directMessage) };

		    if (output.DirectMessage.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.DirectMessage.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_DirectMessages_Edit)]
		 public async Task<GetDirectMessageForEditOutput> GetDirectMessageForEdit(EntityDto input)
         {
            var directMessage = await _directMessageRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetDirectMessageForEditOutput {DirectMessage = ObjectMapper.Map<CreateOrEditDirectMessageDto>(directMessage)};

		    if (output.DirectMessage.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.DirectMessage.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditDirectMessageDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_DirectMessages_Create)]
		 private async Task Create(CreateOrEditDirectMessageDto input)
         {
            var directMessage = ObjectMapper.Map<DirectMessage>(input);

			

            await _directMessageRepository.InsertAsync(directMessage);
         }

		 [AbpAuthorize(AppPermissions.Pages_DirectMessages_Edit)]
		 private async Task Update(CreateOrEditDirectMessageDto input)
         {
            var directMessage = await _directMessageRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, directMessage);
         }

		 [AbpAuthorize(AppPermissions.Pages_DirectMessages_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _directMessageRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetDirectMessagesToExcel(GetAllDirectMessagesForExcelInput input)
         {
			
			var filteredDirectMessages = _directMessageRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.text.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.textFilter),  e => e.text.ToLower() == input.textFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name.ToLower() == input.UserNameFilter.ToLower().Trim());

			var query = (from o in filteredDirectMessages
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetDirectMessageForViewDto() { 
							DirectMessage = new DirectMessageDto
							{
                                text = o.text,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString()
						 });


            var directMessageListDtos = await query.ToListAsync();

            return _directMessagesExcelExporter.ExportToFile(directMessageListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_DirectMessages)]
         public async Task<PagedResultDto<DirectMessageUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<DirectMessageUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new DirectMessageUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<DirectMessageUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}
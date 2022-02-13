using ERP.Entities;
using ERP.Authorization.Users;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Entities.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.Entities
{
	[AbpAuthorize(AppPermissions.Pages_Comments)]
    public class CommentsAppService : ERPAppServiceBase, ICommentsAppService
    {
		 private readonly IRepository<Comment> _commentRepository;
		 private readonly IRepository<Poll,int> _lookup_pollRepository;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public CommentsAppService(IRepository<Comment> commentRepository , IRepository<Poll, int> lookup_pollRepository, IRepository<User, long> lookup_userRepository) 
		  {
			_commentRepository = commentRepository;
			_lookup_pollRepository = lookup_pollRepository;
		_lookup_userRepository = lookup_userRepository;
		
		  }

		 public async Task<PagedResultDto<GetCommentForViewDto>> GetAll(GetAllCommentsInput input)
         {
			
			var filteredComments = _commentRepository.GetAll().Where(x=>x.PollId==input.PollId)
						.Include( e => e.PollFk)
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.text.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.textFilter),  e => e.text.ToLower() == input.textFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.PollTitleFilter), e => e.PollFk != null && e.PollFk.Title.ToLower() == input.PollTitleFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name.ToLower() == input.UserNameFilter.ToLower().Trim());

			var pagedAndFilteredComments = filteredComments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var comments = from o in pagedAndFilteredComments
                         join o1 in _lookup_pollRepository.GetAll() on o.PollId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetCommentForViewDto() {
							Comment = new CommentDto
							{
                                text = o.text,
                                Id = o.Id,
                                PollId=o.PollId
							},
                         	PollTitle = s1 == null ? "" : s1.Title.ToString(),
                         	UserName = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredComments.CountAsync();

            return new PagedResultDto<GetCommentForViewDto>(
                totalCount,
                await comments.ToListAsync()
            );
         }
		 
		 public async Task<GetCommentForViewDto> GetCommentForView(int id)
         {
            var comment = await _commentRepository.GetAsync(id);

            var output = new GetCommentForViewDto { Comment = ObjectMapper.Map<CommentDto>(comment) };

		    if (output.Comment.PollId != null)
            {
                var _lookupPoll = await _lookup_pollRepository.FirstOrDefaultAsync((int)output.Comment.PollId);
                output.PollTitle = _lookupPoll.Title.ToString();
            }

		    if (output.Comment.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Comment.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Comments_Edit)]
		 public async Task<GetCommentForEditOutput> GetCommentForEdit(EntityDto input)
         {
            var comment = await _commentRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCommentForEditOutput {Comment = ObjectMapper.Map<CreateOrEditCommentDto>(comment)};

		    if (output.Comment.PollId != null)
            {
                var _lookupPoll = await _lookup_pollRepository.FirstOrDefaultAsync((int)output.Comment.PollId);
                output.PollTitle = _lookupPoll.Title.ToString();
            }

		    if (output.Comment.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Comment.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCommentDto input)
         {
            var loggedUserId = Convert.ToInt32(AbpSession.UserId);
            if (input.Id == null){
                input.UserId = loggedUserId;
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Comments_Create)]
		 private async Task Create(CreateOrEditCommentDto input)
         {
            var comment = ObjectMapper.Map<Comment>(input);

			

            await _commentRepository.InsertAsync(comment);
         }

		 [AbpAuthorize(AppPermissions.Pages_Comments_Edit)]
		 private async Task Update(CreateOrEditCommentDto input)
         {
            var comment = await _commentRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, comment);
         }

		 [AbpAuthorize(AppPermissions.Pages_Comments_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _commentRepository.DeleteAsync(input.Id);
         } 

		[AbpAuthorize(AppPermissions.Pages_Comments)]
         public async Task<PagedResultDto<CommentPollLookupTableDto>> GetAllPollForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_pollRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Title.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var pollList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<CommentPollLookupTableDto>();
			foreach(var poll in pollList){
				lookupTableDtoList.Add(new CommentPollLookupTableDto
				{
					Id = poll.Id,
					DisplayName = poll.Title?.ToString()
				});
			}

            return new PagedResultDto<CommentPollLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Comments)]
         public async Task<PagedResultDto<CommentUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<CommentUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new CommentUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<CommentUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}
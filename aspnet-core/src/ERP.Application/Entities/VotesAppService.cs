using ERP.Entities;


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
	[AbpAuthorize(AppPermissions.Pages_Votes)]
    public class VotesAppService : ERPAppServiceBase, IVotesAppService
    {
		 private readonly IRepository<Vote> _voteRepository;
		 private readonly IRepository<Poll,int> _lookup_pollRepository;
		 

		  public VotesAppService(IRepository<Vote> voteRepository , IRepository<Poll, int> lookup_pollRepository) 
		  {
			_voteRepository = voteRepository;
			_lookup_pollRepository = lookup_pollRepository;
		
		  }

		 public async Task<PagedResultDto<GetVoteForViewDto>> GetAll(GetAllVotesInput input)
         {
			
			var filteredVotes = _voteRepository.GetAll()
						.Include( e => e.PollFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.PollTitleFilter), e => e.PollFk != null && e.PollFk.Title.ToLower() == input.PollTitleFilter.ToLower().Trim());

			var pagedAndFilteredVotes = filteredVotes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var votes = from o in pagedAndFilteredVotes
                         join o1 in _lookup_pollRepository.GetAll() on o.PollId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetVoteForViewDto() {
							Vote = new VoteDto
							{
                                Id = o.Id
							},
                         	PollTitle = s1 == null ? "" : s1.Title.ToString()
						};

            var totalCount = await filteredVotes.CountAsync();

            return new PagedResultDto<GetVoteForViewDto>(
                totalCount,
                await votes.ToListAsync()
            );
         }
		 
		 public async Task<GetVoteForViewDto> GetVoteForView(int id)
         {
            var vote = await _voteRepository.GetAsync(id);

            var output = new GetVoteForViewDto { Vote = ObjectMapper.Map<VoteDto>(vote) };

		    if (output.Vote.PollId != null)
            {
                var _lookupPoll = await _lookup_pollRepository.FirstOrDefaultAsync((int)output.Vote.PollId);
                output.PollTitle = _lookupPoll.Title.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Votes_Edit)]
		 public async Task<GetVoteForEditOutput> GetVoteForEdit(EntityDto input)
         {
            var vote = await _voteRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetVoteForEditOutput {Vote = ObjectMapper.Map<CreateOrEditVoteDto>(vote)};

		    if (output.Vote.PollId != null)
            {
                var _lookupPoll = await _lookup_pollRepository.FirstOrDefaultAsync((int)output.Vote.PollId);
                output.PollTitle = _lookupPoll.Title.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditVoteDto input)
         {
            var logedUserId = Convert.ToInt32(AbpSession.UserId);
             var item=  _voteRepository.GetAll().FirstOrDefault(x => x.PollId == input.PollId && x.CreatorUserId == logedUserId);
             
            if (item==null)
            {
				await Create(input);
			}
			else{
                input.Id = item.Id;
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Votes_Create)]
		 private async Task Create(CreateOrEditVoteDto input)
         {
            var vote = ObjectMapper.Map<Vote>(input);

			

            await _voteRepository.InsertAsync(vote);
         }

		 [AbpAuthorize(AppPermissions.Pages_Votes_Edit)]
		 private async Task Update(CreateOrEditVoteDto input)
         {
            var vote = await _voteRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, vote);
         }

		 [AbpAuthorize(AppPermissions.Pages_Votes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _voteRepository.DeleteAsync(input.Id);
         } 

		[AbpAuthorize(AppPermissions.Pages_Votes)]
         public async Task<PagedResultDto<VotePollLookupTableDto>> GetAllPollForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_pollRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Title.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var pollList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<VotePollLookupTableDto>();
			foreach(var poll in pollList){
				lookupTableDtoList.Add(new VotePollLookupTableDto
				{
					Id = poll.Id,
					DisplayName = poll.Title?.ToString()
				});
			}

            return new PagedResultDto<VotePollLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}
using Abp.Application.Services.Dto;
using System;

namespace ERP.Entities.Dtos
{
    public class GetAllVotesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string PollTitleFilter { get; set; }

		 
    }
}
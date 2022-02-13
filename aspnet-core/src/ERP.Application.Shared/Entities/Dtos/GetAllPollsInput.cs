using Abp.Application.Services.Dto;
using System;

namespace ERP.Entities.Dtos
{
    public class GetAllPollsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string TitleFilter { get; set; }

		public string Option1Filter { get; set; }

		public string Option2Filter { get; set; }

		public string Option3Filter { get; set; }

		public string Option4Filter { get; set; }



    }
}
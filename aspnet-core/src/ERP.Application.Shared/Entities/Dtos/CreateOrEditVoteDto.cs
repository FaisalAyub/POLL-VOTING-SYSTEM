
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities.Dtos
{
    public class CreateOrEditVoteDto : EntityDto<int?>
    {

		public int? selectedOption { get; set; }
		
		
		 public int? PollId { get; set; }
		 
		 
    }
}
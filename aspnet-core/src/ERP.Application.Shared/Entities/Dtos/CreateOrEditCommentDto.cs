
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities.Dtos
{
    public class CreateOrEditCommentDto : EntityDto<int?>
    {

		public string text { get; set; }
		
		
		 public int? PollId { get; set; }
		 
		 		 public long? UserId { get; set; }
		 
		 
    }
}

using System;
using Abp.Application.Services.Dto;

namespace ERP.Entities.Dtos
{
    public class CommentDto : EntityDto
    {
		public string text { get; set; }


		 public int? PollId { get; set; }

		 		 public long? UserId { get; set; }

		 
    }
}
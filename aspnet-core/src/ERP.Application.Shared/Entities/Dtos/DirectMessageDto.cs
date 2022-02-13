
using System;
using Abp.Application.Services.Dto;

namespace ERP.Entities.Dtos
{
    public class DirectMessageDto : EntityDto
    {
		public string text { get; set; }


		 public long? UserId { get; set; }

		 
    }
}
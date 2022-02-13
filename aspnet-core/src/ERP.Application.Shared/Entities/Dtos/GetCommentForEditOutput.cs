using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities.Dtos
{
    public class GetCommentForEditOutput
    {
		public CreateOrEditCommentDto Comment { get; set; }

		public string PollTitle { get; set;}

		public string UserName { get; set;}


    }
}
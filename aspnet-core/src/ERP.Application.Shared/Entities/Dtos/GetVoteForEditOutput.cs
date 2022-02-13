using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities.Dtos
{
    public class GetVoteForEditOutput
    {
		public CreateOrEditVoteDto Vote { get; set; }

		public string PollTitle { get; set;}


    }
}
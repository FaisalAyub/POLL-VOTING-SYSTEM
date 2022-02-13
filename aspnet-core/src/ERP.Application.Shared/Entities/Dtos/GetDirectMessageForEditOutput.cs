using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities.Dtos
{
    public class GetDirectMessageForEditOutput
    {
		public CreateOrEditDirectMessageDto DirectMessage { get; set; }

		public string UserName { get; set;}


    }
}
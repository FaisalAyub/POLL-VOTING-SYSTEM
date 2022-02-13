using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities.Dtos
{
    public class GetPollForEditOutput
    {
		public CreateOrEditPollDto Poll { get; set; }


    }
}
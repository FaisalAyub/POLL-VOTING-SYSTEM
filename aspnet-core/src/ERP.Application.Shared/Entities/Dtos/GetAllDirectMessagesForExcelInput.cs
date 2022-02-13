using Abp.Application.Services.Dto;
using System;

namespace ERP.Entities.Dtos
{
    public class GetAllDirectMessagesForExcelInput
    {
		public string Filter { get; set; }

		public string textFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 
    }
}
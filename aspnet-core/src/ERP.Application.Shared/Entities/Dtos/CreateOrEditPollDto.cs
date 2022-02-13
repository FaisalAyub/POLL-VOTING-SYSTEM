
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities.Dtos
{
    public class CreateOrEditPollDto : EntityDto<int?>
    {

		public string Title { get; set; }
		
		
		public string Option1 { get; set; }
		
		
		public string Option2 { get; set; }
		
		
		public string Option3 { get; set; }
		
		
		public string Option4 { get; set; }
		
		
		public int? count1 { get; set; }
		
		
		public int? count2 { get; set; }
		
		
		public int? count3 { get; set; }
		
		
		public int? count4 { get; set; }
		
		
		 public long? UserId { get; set; }
		 
		 
    }
}
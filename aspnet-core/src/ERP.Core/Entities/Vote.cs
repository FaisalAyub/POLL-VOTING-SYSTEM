using ERP.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Entities
{
	[Table("Votes")]
    public class Vote : FullAuditedEntity 
    {

		public virtual int? selectedOption { get; set; }
		

		public virtual int? PollId { get; set; }
		
        [ForeignKey("PollId")]
		public Poll PollFk { get; set; }
		
    }
}
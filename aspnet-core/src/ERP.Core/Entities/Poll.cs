using ERP.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Entities
{
	[Table("Polls")]
    public class Poll : FullAuditedEntity 
    {

		public virtual string Title { get; set; }
		
		public virtual string Option1 { get; set; }
		
		public virtual string Option2 { get; set; }
		
		public virtual string Option3 { get; set; }
		
		public virtual string Option4 { get; set; }
		

		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
    }
}
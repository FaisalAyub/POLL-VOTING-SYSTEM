using ERP.Entities;
using ERP.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Entities
{
	[Table("Comments")]
    public class Comment : FullAuditedEntity 
    {

		public virtual string text { get; set; }
		

		public virtual int? PollId { get; set; }
		
        [ForeignKey("PollId")]
		public Poll PollFk { get; set; }
		
		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
    }
}
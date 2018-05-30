using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAPSimple.Models
{
    public class Invite
    {
        public int ID { get; set; }
        [Column("Status")]
        public bool Status { get; set; }
        public int ProfileId { get; set; }
        public virtual Profile Profile {get;set;}
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
        //[Display(Name = "статус")]
        //       [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
    }
}
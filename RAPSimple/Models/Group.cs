using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RAPSimple.Models
{
    public class Group
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Profile> Profiles { get; set; }
        
        public Group ()
        {
            Profiles = new List<Profile>();
        }
    }
}
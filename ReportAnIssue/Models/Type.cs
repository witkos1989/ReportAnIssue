using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReportAnIssue.Models
{
    public class Type : IEntity
    {
        [Required]
        public string Name { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
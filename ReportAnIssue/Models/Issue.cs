using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReportAnIssue.Models
{
    public class Issue : IEntity
    {
        [Required]
        public string Title { get; set; }
        public string InventoryNumber { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(1, 6)]
        public byte Rate { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public string RateComment { get; set; }
        public byte State { get; set; }
        //public string Attachment { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int TypeId { get; set; }
        public int FileId { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
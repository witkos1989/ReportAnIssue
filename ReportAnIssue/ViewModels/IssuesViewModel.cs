using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportAnIssue.ViewModels
{
    public class IssuesViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string InventoryNumber { get; set; }
        [Required]
        public int TypeId { get; set; }
        public string userName { get; set; }
        public DateTime StartDate { get; set; }
        public string TimeElapsed { get; set; }
        [Required]
        public string Description { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public ReportAnIssue.Models.File Attachment { get; set; } 
    }
}
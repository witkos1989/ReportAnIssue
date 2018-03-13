using ReportAnIssue.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportAnIssue.ViewModels
{
    public class IssueDetailsViewModel
    {
        public Issue Issue { get; set; }
        public int Id { get; set; }
        [Required]
        public string CommentContent { get; set; }
        public List<CommentsViewModel> Comments { get; set; }
        public ReportAnIssue.Models.Type Type { get; set; }
        public IEnumerable<SelectListItem> Rates { get; set; }
        public string TimeExpired { get; set; }
        public string UserId { get; set; }
        public ReportAnIssue.Models.File Attachment { get; set; }

        [Required]
        [Range(1, 6)]
        public byte Rate { get; set; }

        [Required]
        public string CommentRate { get; set; }

        public IssueDetailsViewModel()
        {
            Comments = new List<CommentsViewModel>();
        }
    }
}
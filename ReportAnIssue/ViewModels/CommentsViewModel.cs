using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportAnIssue.ViewModels
{
    public class CommentsViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
    }
}
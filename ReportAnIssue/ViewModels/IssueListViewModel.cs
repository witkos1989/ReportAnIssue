using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportAnIssue.ViewModels
{
    public class IssueListViewModel
    {
        public List<IssuesViewModel> IssuesVM { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public IssueListViewModel()
        {
            IssuesVM = new List<IssuesViewModel>();
        }
    }
}
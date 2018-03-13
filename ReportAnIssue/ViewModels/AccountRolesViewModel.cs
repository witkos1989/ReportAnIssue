using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportAnIssue.ViewModels
{
    public class AccountRolesViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public List<string> RolesList { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        public string TypeString { get; set; }
        public ReportAnIssue.Models.Type Type { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
    }
}
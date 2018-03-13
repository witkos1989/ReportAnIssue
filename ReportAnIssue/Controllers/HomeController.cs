using Microsoft.AspNet.Identity;
using ReportAnIssue.Extensions;
using ReportAnIssue.Models;
using ReportAnIssue.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportAnIssue.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (User.Identity.OneTimePassword())
                return RedirectToAction("ChangePassword", "Manage");
            return View(IssuesIndex(true));
        }

        [Authorize]
        public ActionResult IssueArchive()
        {
            if (User.Identity.OneTimePassword())
                return RedirectToAction("ChangePassword", "Manage");
            return View(IssuesIndex(false));
        }

        [Authorize]
        public ActionResult DeletedIssues()
        {
            if (User.Identity.OneTimePassword())
                return RedirectToAction("ChangePassword", "Manage");
            return View(IssuesIndex(false, true));
        }

        private IssueListViewModel IssuesIndex(bool isActive, bool deleted = false)
        {
            var issues = new IssueListViewModel();
            List<Issue> issuesList = new List<Issue>();
            var userId = User.Identity.GetUserId();
            if (isActive)
                issuesList = db.Issues.Where(i => i.State == 0 || i.State == 1).ToList();
            else
                if (!deleted)
                issuesList = db.Issues.Where(i => i.State == 2).ToList();
            else
                issuesList = db.Issues.Where(i => i.State == 3).ToList();
            if(!User.IsInRole("Admin"))
            {
                var filteredIssues = new List<Issue>();
                if(User.IsInRole("ShowInformatics"))
                {
                    filteredIssues.AddRange(issuesList.Where(i => i.TypeId == 1 && i.UserId != userId).ToList());
                }
                if (User.IsInRole("ShowHydraulics"))
                {
                    filteredIssues.AddRange(issuesList.Where(i => i.TypeId == 2 && i.UserId != userId).ToList());
                }
                if (User.IsInRole("ShowElectrics"))
                {
                    filteredIssues.AddRange(issuesList.Where(i => i.TypeId == 3 && i.UserId != userId).ToList());
                }
                if (User.IsInRole("ShowBuilding"))
                {
                    filteredIssues.AddRange(issuesList.Where(i => i.TypeId == 4 && i.UserId != userId).ToList());
                }
                if (User.IsInRole("ShowOrdinal"))
                {
                    filteredIssues.AddRange(issuesList.Where(i => i.TypeId == 5 && i.UserId != userId).ToList());
                }
                if (User.IsInRole("ShowPhones"))
                {
                    filteredIssues.AddRange(issuesList.Where(i => i.TypeId == 6 && i.UserId != userId).ToList());
                }
                if (User.IsInRole("ShowResearch"))
                {
                    filteredIssues.AddRange(issuesList.Where(i => i.TypeId == 7 && i.UserId != userId).ToList());
                }
                if (User.IsInRole("ShowOthers"))
                {
                    filteredIssues.AddRange(issuesList.Where(i => i.TypeId == 8 && i.UserId != userId).ToList());
                }
                if (User.IsInRole("User"))
                {
                    filteredIssues.AddRange(issuesList.Where(i => i.UserId == userId).ToList());
                }
                issuesList.Clear();
                issuesList.AddRange(filteredIssues);
            }
            if (issuesList.Count != 0)
                foreach (var item in issuesList)
                {
                    var model = new IssuesViewModel();
                    model.Id = item.Id;
                    model.Title = item.Title;
                    if (db.Users.Single(u => u.Id == item.UserId) != null)
                        model.userName = db.Users.Single(u => u.Id == item.UserId).UserName;
                    else
                        model.userName = "użytkownik usunięty";
                    model.TypeId = item.TypeId;
                    model.StartDate = item.StartDate;
                    model.TimeElapsed = String.Format("{0:%d}d {0:%h}g {0:%m}m {0:%s}s", DateTime.Now - item.StartDate);
                    model.InventoryNumber = item.InventoryNumber;
                    issues.IssuesVM.Add(model);
                }
            issues.Types = db.Types.Where(t => t.Active).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            return issues;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddIssue()
        {
            ViewBag.Message = "Dodaj nową usterkę";

            return View();
        }
    }
}
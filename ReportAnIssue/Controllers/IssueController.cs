using Microsoft.AspNet.Identity;
using ReportAnIssue.Extensions;
using ReportAnIssue.Global;
using ReportAnIssue.Models;
using ReportAnIssue.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ReportAnIssue.Controllers
{
    public class IssueController : BaseController
    {
        private bool CheckPermissions(Issue issue)
        {
            if (User.IsInRole("Admin"))
                return true;
            if (User.IsInRole("User") && issue.UserId == User.Identity.GetUserId())
                return true;
            if (User.IsInRole("ShowBuilding") && issue.TypeId == 4)
                return true;
            if (User.IsInRole("ShowInformatics") && issue.TypeId == 1)
                return true;
            if (User.IsInRole("ShowElectrics") && issue.TypeId == 3)
                return true;
            if (User.IsInRole("ShowHydraulics") && issue.TypeId == 2)
                return true;
            if (User.IsInRole("ShowOrdinal") && issue.TypeId == 5)
                return true;
            if (User.IsInRole("ShowOthers") && issue.TypeId == 8)
                return true;
            if (User.IsInRole("ShowPhones") && issue.TypeId == 6)
                return true;
            if (User.IsInRole("ShowResearch") && issue.TypeId == 7)
                return true;
            return false;
        }
        // GET: Issue/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (User.Identity.OneTimePassword())
                return RedirectToAction("ChangePassword", "Manage");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue issue = db.Issues.Find(id);
            var rate = new List<int>() { 1, 2, 3, 4, 5, 6 };
            if (issue == null)
            {
                return HttpNotFound();
            }
            if (!CheckPermissions(issue))
            {
                TempData["Fail"] = "Nie posiadasz uprawnień do edycji tej usterki!";
                return RedirectToAction("Index", "Home");
            }

            var issueDetails = new IssueDetailsViewModel();
            issueDetails.Issue = issue;
            foreach (var item in issueDetails.Issue.Comments)
            {
                var comment = new CommentsViewModel();
                comment.Content = item.Content;
                comment.Date = item.Date;
                if (db.Users.Single(u => u.Id == item.UserId) != null)
                    comment.UserName = db.Users.Single(u => u.Id == item.UserId).UserName;
                else
                    comment.UserName = "użytkownik usunięty";
                issueDetails.Comments.Add(comment);
            }
            if (issue.State == 2)
                issueDetails.TimeExpired = String.Format("{0:%d}d {0:%h}g {0:%m}m {0:%s}s", issue.EndDate - issue.StartDate);
            issueDetails.Rates = rate.Select(c => new SelectListItem { Text = c.ToString(), Value = c.ToString() });
            issueDetails.CommentRate = "Usterka naprawiona";
            issueDetails.Id = issue.Id;
            issueDetails.Type = db.Types.First(t => t.Id == issue.TypeId);
            issueDetails.UserId = User.Identity.GetUserId();
            if (issue.FileId != 0)
            {
                var attachment = db.Files.FirstOrDefault(f => f.Id == issue.FileId);
                if (attachment != null)
                    issueDetails.Attachment = attachment;
            }
            return View(issueDetails);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(IssueDetailsViewModel issueDetails)
        {
            Issue issue = db.Issues.Find(issueDetails.Id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            var comm = new Comment();
            comm.UserId = User.Identity.GetUserId();
            comm.Content = issueDetails.CommentContent;
            comm.Date = DateTime.Now;
            issue.Comments.Add(comm);
            db.SaveChanges();
            var userMails = new List<string>();
            var userId = User.Identity.GetUserId();
            userMails.Add(db.Users.First(u => u.Id == userId).Email);
            var type = db.Types.First(t => t.Id == issue.TypeId);
            foreach (var user in type.Users)
                userMails.Add(user.Email);
            SendMail.InformUsers(userMails, issue, User.Identity.Name, comm.Content, 2);
            TempData["Success"] = "Dodano nowy komentarz!";
            return RedirectToAction("Details", "Issue", new { id = issueDetails.Id });
        }

        [Authorize]
        [HttpPost]
        public ActionResult DownloadAttachment(IssueDetailsViewModel attachment)
        {
            var fileId = attachment.Issue.FileId;
            var file = db.Files.FirstOrDefault(f => f.Id == fileId);
            if (file != null)
            {
                string filename = file.FileName;
                byte[] filedata = file.Content;
                string contentType = file.ContentType;

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = filename,
                    Inline = true,
                };

                Response.AppendHeader("Content-Disposition", cd.ToString());

                return File(filedata, contentType);
            }
            TempData["Fail"] = "Taki plik nie istnieje w bazie!";
            return RedirectToAction("Details", "Issue", new { id = attachment.Id });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RateIssue(IssueDetailsViewModel issueDetails)
        {
            Issue issue = db.Issues.Find(issueDetails.Id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            issue.Rate = issueDetails.Rate;
            issue.RateComment = issueDetails.CommentRate;
            issue.State = 2;
            issue.EndDate = DateTime.Now;
            db.SaveChanges();
            var userMails = new List<string>();
            var userId = User.Identity.GetUserId();
            userMails.Add(db.Users.First(u => u.Id == userId).Email);
            var type = db.Types.First(t => t.Id == issue.TypeId);
            foreach (var user in type.Users)
                userMails.Add(user.Email);
            SendMail.InformUsers(userMails, issue, User.Identity.Name, "", 4);
            TempData["Success"] = "Usterka została oceniona i przeniesiona do archiwum!";
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IssueDone(IssueDetailsViewModel issueDetails)
        {
            Issue issue = db.Issues.Find(issueDetails.Id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            issue.State = 1;
            db.SaveChanges();
            var userMails = new List<string>();
            var userId = User.Identity.GetUserId();
            userMails.Add(db.Users.First(u => u.Id == userId).Email);
            var type = db.Types.First(t => t.Id == issue.TypeId);
            SendMail.InformUsers(userMails, issue, "", "", 3);
            TempData["Success"] = "Usterka została wykonana. Użytkownik został poinformowany!";
            return RedirectToAction("Index", "Home");
        }

        // GET: Issue/Create
        [Authorize]
        public ActionResult Create()
        {
            if (User.Identity.OneTimePassword())
                return RedirectToAction("ChangePassword", "Manage");
            var userId = User.Identity.GetUserId().ToString();
            var notRated = db.Issues.Where(i => i.UserId == userId && i.State == 1).ToList();
            if (notRated.Count > 1)
            {
                TempData["Fail"] = "W systemie istnieją usterki wykonane, ale nie zakończone. Przed dodaniem nowej usterki należy zamknąć poprzednie!";
                return RedirectToAction("Index", "Home");
            }
            var issueVM = new IssuesViewModel();
            issueVM.Types = db.Types.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            return View(issueVM);
        }

        // POST: Issue/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(IssuesViewModel issueVM, HttpPostedFileBase upload)
        {
            var issue = new Issue()
            {
                UserId = User.Identity.GetUserId(),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                InventoryNumber = issueVM.InventoryNumber,
                State = 0,
                Rate = 1,
                Description = issueVM.Description,
                Title = issueVM.Title,
                TypeId = issueVM.TypeId
            };
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var attachment = new ReportAnIssue.Models.File
                    {
                        FileName = System.IO.Path.GetFileName(upload.FileName),
                        ContentType = upload.ContentType
                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        attachment.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    db.Files.Add(attachment);
                    db.SaveChanges();
                    issue.FileId = attachment.Id;
                }
                db.Issues.Add(issue);
                db.SaveChanges();
            }
            var userMails = new List<string>();
            var userId = User.Identity.GetUserId();
            userMails.Add(db.Users.First(u => u.Id == userId).Email);
            var type = db.Types.First(t => t.Id == issue.TypeId);
            foreach (var user in type.Users)
                userMails.Add(user.Email);
            SendMail.InformUsers(userMails, issue, User.Identity.Name, "", 1);
            TempData["Success"] = "Dodano nową usterkę!";
            return RedirectToAction("Index", "Home");
        }

        // GET: Issue/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Issue/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Issue/Delete/5

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue issue = db.Issues.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            return View(issue);
        }

        // POST: Issue/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            Issue issue = db.Issues.Find(id);
            issue.State = 3;
            db.SaveChanges();
            TempData["Success"] = "Usterka została usunięta!";
            return RedirectToAction("Index", "Home");
        }
    }
}

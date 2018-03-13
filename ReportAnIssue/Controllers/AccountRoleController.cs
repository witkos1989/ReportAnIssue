using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ReportAnIssue.Models;
using ReportAnIssue.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportAnIssue.Extensions;

namespace ReportAnIssue.Controllers
{
    public class AccountRoleController : BaseController
    {
        // GET: AccountRoles/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var AccountRoles = new AccountRolesViewModel();
            AccountRoles.Roles = db.Roles.Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.Name.ToString(),
            }).ToList();
            return View(AccountRoles);
        }

        // POST: AccountRoles/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(AccountRolesViewModel AccountRoles)
        {
            ApplicationUser user = db.Users.Where(u => u.Email.Equals(AccountRoles.Email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var userStore = new UserStore<ApplicationUser>(db);
            var account = new UserManager<ApplicationUser>(userStore);

            account.AddToRole(user.Id, AccountRoles.Role);
            db.SaveChanges();
            TempData["Success"] = "Dodano uprawnienie do użytkownika";
            return RedirectToAction("Index", "Manage", null);
        }

        [Authorize(Roles = "Admin")]
        private AccountRolesViewModel SetAccountTypes()
        {
            var accountTypes = new AccountRolesViewModel();
            accountTypes.Email = "";
            accountTypes.Types = db.Types.Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.Name.ToString(),
            }).ToList();
            return accountTypes;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Add(int? id)
        {
            var type = db.Types.FirstOrDefault(t => t.Id == id);
            if (type == null)
            {
                TempData["Fail"] = "Nie ma takiego typu usterki";
                return RedirectToAction("AccountRolesList", "AccountRole");
            }
            return View(type);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Add(int typeId, string userMail)
        {
            var user = db.Users.FirstOrDefault(u => u.Email.Equals(userMail, StringComparison.InvariantCultureIgnoreCase));
            var type = db.Types.FirstOrDefault(t => t.Id == typeId);
            if (user == null)
            {
                ViewBag.Message = "Nie ma takiego użytkownika";
                return View(type);
            }
            type.Users.Add(user);
            db.SaveChanges();
            TempData["Success"] = "Dodano użytkownika";
            return RedirectToAction("AccountRolesList", "AccountRole");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ConfigureEmails()
        {
            return View(SetAccountTypes());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult ConfigureEmails(AccountRolesViewModel AccountRoles)
        {
            ApplicationUser user = db.Users.Where(u => u.Email.Equals(AccountRoles.Email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (user == null)
            {
                ViewBag.Message = "Nie ma takiego użytkownika";
                return View(SetAccountTypes());
            }
            ReportAnIssue.Models.Type type = db.Types.FirstOrDefault(t => t.Name.Equals(AccountRoles.TypeString));
            if (type == null)
            {
                ViewBag.Message = "Nie ma takiego typu usterki";
                return View(SetAccountTypes());
            }
            if (type.Users.Contains(user))
            {
                ViewBag.Message = "Ten użytkownik otrzymuje już powiadomienia z tego typu usterki";
                return View(SetAccountTypes());
            }
            type.Users.Add(user);
            db.SaveChanges();
            ViewBag.Success = "Dodano mail do listy";
            return View(SetAccountTypes());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AccountRolesList()
        {
            return View(db.Types.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteAccount(string userId, int typeId)
        {
            var type = db.Types.FirstOrDefault(t => t.Id == typeId);
            if (type == null)
            {
                TempData["Fail"] = "Nie ma takiego typu usterki";
                return RedirectToAction("AccountRolesList", "AccountRole");
            }

            var user = db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                TempData["Fail"] = "Nie ma takiego użytkownika";
                return RedirectToAction("AccountRolesList", "AccountRole");
            }

            if (!type.Users.Contains(user))
            {
                TempData["Fail"] = "Użytkownika nie ma na tej liście";
                return RedirectToAction("AccountRolesList", "AccountRole");
            }

            type.Users.Remove(user);
            db.SaveChanges();
            TempData["Success"] = "Użytkownik usunięty z listy";
            return RedirectToAction("AccountRolesList", "AccountRole");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(AccountRolesViewModel AccountRoles)
        {
            ApplicationUser user = db.Users.Where(u => u.Email.Equals(AccountRoles.Email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var userStore = new UserStore<ApplicationUser>(db);
            var account = new UserManager<ApplicationUser>(userStore);

            if (account.IsInRole(user.Id, AccountRoles.Role))
                account.RemoveFromRole(user.Id, AccountRoles.Role);
            db.SaveChanges();
            TempData["Success"] = "Usunięto uprawnienie użytkownikowi";
            return RedirectToAction("Index", "Manage", null);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details()
        {
            var AccountRoless = new List<AccountRolesViewModel>();
            List<ApplicationUser> users = db.Users.ToList();
            var userStore = new UserStore<ApplicationUser>(db);
            var account = new UserManager<ApplicationUser>(userStore);
            foreach (var user in users)
            {
                var AccountRoles = new AccountRolesViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    RolesList = account.GetRoles(user.Id).ToList(),
                };
                AccountRoless.Add(AccountRoles);
            }

            return View(AccountRoless);
        }
    }
}

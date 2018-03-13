using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportAnIssue.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TypeController : BaseController
    {
        public ActionResult Index()
        {
            var types = db.Types.ToList();
            return View(types);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ReportAnIssue.Models.Type type)
        {
            type.Active = true;
            db.Types.Add(type);
            db.SaveChanges();
            return RedirectToAction("Index", "Manage");
        }

        public ActionResult Edit(int id)
        {
            var type = db.Types.FirstOrDefault(t => t.Id == id);
            if (type != null)
                return View(type);
            else
                return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        public ActionResult Edit(ReportAnIssue.Models.Type type)
        {
            if(ModelState.IsValid)
            {
                db.Entry(type).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Type");
            }
            return View(type);
        }
    }
}

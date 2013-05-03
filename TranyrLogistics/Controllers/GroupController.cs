using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using TranyrLogistics.Models;

namespace TranyrLogistics.Controllers
{
    public class GroupController : Controller
    {
        private TranyrLogisticsDb db = new TranyrLogisticsDb();

        //
        // GET: /Group/

        [Authorize(Roles = "Administrator, Finance, Manager")]
        public ActionResult Index()
        {
            return View(db.Groups.ToList());
        }

        //
        // GET: /Group/Details/5

        [Authorize(Roles = "Administrator, Finance, Manager")]
        public ActionResult Details(int id = 0)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        //
        // GET: /Group/Create

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Group/Create

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create(Group group)
        {
            group.CreateDate = group.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        //
        // GET: /Group/Edit/5

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int id = 0)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        //
        // POST: /Group/Edit/5

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(Group group)
        {
            using (TranyrLogisticsDb db = new TranyrLogisticsDb())
            {
                var currentGroup = db.Groups.Find(group.ID);
                group.CreateDate = currentGroup.CreateDate;
            }
            group.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        //
        // GET: /Group/Delete/5

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(int id = 0)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        //
        // POST: /Group/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
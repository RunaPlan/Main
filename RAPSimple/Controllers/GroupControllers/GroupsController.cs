using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RAPSimple.DAL;
using RAPSimple.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using RAPSimple.Library;

namespace RAPSimple.Controllers.GroupControllers
{
    public class Testx
    {
        public Testx()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().
            FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            namer=user.Email;
        }
        public string namer = "xsdada";
    }


    public class GroupsController : Controller
    {
        private RAPDbContext db = new RAPDbContext();
        private Profile centralCurrentProfile = ProfileProcedure.getCurrentProfile();

        public ActionResult Test()
        {
            Testx zzz = new Testx();
            return View(zzz);
        }



        // GET: Groups
        public ActionResult Index()
        {
            
            
            string mail = ProfileProcedure.getCurentUserMail();
            Profile currentProfile = db.Profiles.Where(x => x.Email == mail).First();
            
            //return View(db.Groups.ToList());все группы
            return View(currentProfile.Groups);
        }

        // GET: Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description")] Group group)
        {
            if (ModelState.IsValid)
            {
                string mail = ProfileProcedure.getCurentUserMail();
                Profile currentProfile = db.Profiles.Where(x => x.Email == mail).First();
                group.Profiles.Add(currentProfile);
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string mail = ProfileProcedure.getCurentUserMail();
            Profile currentProfile = db.Profiles.Where(x => x.Email == mail).First();
            Group group = db.Groups.Find(id);
            currentProfile.Groups.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

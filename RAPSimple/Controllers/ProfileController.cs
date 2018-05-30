using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using PagedList;

using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using RAPSimple.Models;
using RAPSimple.DAL;

namespace RAPSimple.Controllers
{
    public class ProfileController : Controller
    {
        private RAPDbContext db = new RAPDbContext();

        public static string GetUserID(string email)
        {
            RAPDbContext db0 = new RAPDbContext();
            var Profiles = from s in db0.Profiles
                           where s.Email.Equals(email)
                           select s;
            if (Profiles.Count() == 0)
            {
               return "-1";
            }
            else
            {
               return  Profiles.First().ID.ToString();//.f.ElementAt(0).ID.ToString();
            }
        }
#if false
        // GET: Profile
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var Persons = from s in db.Profiles
                          select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                Persons = Persons.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstMidName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    Persons = Persons.OrderByDescending(s => s.LastName);
                    break;
                default:  // Name ascending 
                    Persons = Persons.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(Persons.ToPagedList(pageNumber, pageSize));
            //         return View(Persons.ToList());
        }

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile Profile = db.Profiles.Include(s => s.Files).SingleOrDefault(s => s.ID == id);
            if (Profile == null)
            {
                return HttpNotFound();
            }
            return View(Profile);
        }
#endif
        public ActionResult Index()
        {
            return View();
        }

        // GET: Student/Create
        public ActionResult Create()
        {
 //           Profile prf = new Profile();
 //           prf.Email = User.Identity.Name;
            ViewBag.Email = User.Identity.Name;
//            return View(prf);
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName, FirstMidName, Details, MobilePhone, Email, CommonSkill, TrackingSkill, BikeSkill, RaftSkill")]Profile Profile, HttpPostedFileBase upload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        var avatar = new File
                        {
                            FileName = System.IO.Path.GetFileName(upload.FileName),
                            FileType = FileType.Avatar,
                            ContentType = upload.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            avatar.Content = reader.ReadBytes(upload.ContentLength);
                        }
                        Profile.Files = new List<File> { avatar };
                    }
                    Profile.Email = User.Identity.Name;  // поле подставляется автоматически из идентификации
                    db.Profiles.Add(Profile);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(Profile);
        }

        // GET
        public ActionResult Edit(string sid)
        {
            int id;
            if (sid == "-1")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            id = int.Parse(sid);
            Profile Profile = db.Profiles.Include(s => s.Files).SingleOrDefault(s => s.ID == id);
            if (Profile == null)
            {
                return HttpNotFound();
            }
            return View(Profile);
        }

        // POST:
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, HttpPostedFileBase upload)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var PeopleToUpdate = db.Profiles.Find(id);
            if (TryUpdateModel(PeopleToUpdate, "",
               new string[] { "LastName, FirstMidName, Details, MobilePhone, Email, CommonSkill, TrackingSkill, BikeSkill, RaftSkill" }))
//               new string[] { "LastName", "FirstMidName", "Details" }))
            //LastName, FirstMidName, Details, MobilePhone, Email,CommonSkill, TrackingSkill, BikeSkill, RaftSkill
            {
                try
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        if (PeopleToUpdate.Files.Any(f => f.FileType == FileType.Avatar))
                        {
                            db.Files.Remove(PeopleToUpdate.Files.First(f => f.FileType == FileType.Avatar));
                        }
                        var avatar = new File
                        {
                            FileName = System.IO.Path.GetFileName(upload.FileName),
                            FileType = FileType.Avatar,
                            ContentType = upload.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            avatar.Content = reader.ReadBytes(upload.ContentLength);
                        }
                        PeopleToUpdate.Files = new List<File> { avatar };
                    }
                    db.Entry(PeopleToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(PeopleToUpdate);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Profile Profile = db.Profiles.Find(id);
            if (Profile == null)
            {
                return HttpNotFound();
            }
            return View(Profile);
        }

        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Profile Profile = db.Profiles.Find(id);
                db.Profiles.Remove(Profile);
                db.SaveChanges();
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
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
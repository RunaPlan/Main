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
    public class InviteController : Controller
    {
        private RAPDbContext db = new RAPDbContext();
 
        // GET: Invite
        public ActionResult Index(string sid)
        {
            if (null != sid)
            {
                //              ParentId = int.Parse(sid);
                Session["ParentID"] = sid;
            }

//            int SelectedProfile;
#if false
            var departments = db.Departments.OrderBy(q => q.Name).ToList();
            ViewBag.SelectedDepartment = new SelectList(departments, "DepartmentID", "Name", SelectedDepartment);
            int departmentID = SelectedDepartment.GetValueOrDefault();
#endif

            PopulateEventDropDownList();
#if false
            IQueryable<Invite> inv = db.Invites
                .Where(c =>  c.PDepartmentID == departmentID)
                .OrderBy(d => d.CourseID)
                .Include(d => d.Department);
            var sql = courses.ToString();
#endif
//            return View(courses.ToList());
            var invites = from s in db.Invites
                         select s;
            invites = invites.OrderBy(s => s.EventId);

// здесь надо отфильтровать invites по текущему пользователю!!!
            int pageSize = 3;
            int? page = null;
            page = 1;
            int pageNumber = (page ?? 1);
            return View(invites.ToPagedList(pageNumber, pageSize));
//            return View(db.Invites.ToList());
        }


        // Мои приглашения
        // GET
#if false
        public ActionResult Edit(string sid)
        {
            int id;
            if (sid == "-1")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            id = int.Parse(sid);
            Invite inv = db.Invites.Include(s => s.Files).SingleOrDefault(s => s.ID == id);
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

#endif


        private void PopulateProfileDropDownList(object selectedProfile = null)
        {
            var profileQuery = from d in db.Profiles
                               orderby d.FullName
                               select d;
            ViewBag.ProfileID = new SelectList(profileQuery, "ProfileID", "FullName", selectedProfile);
        }

        private void PopulateEventDropDownList(object selectedEvent = null)
        {
            var eventQuery = from d in db.Events
                               orderby d.Title
                               select d;
            ViewBag.EventID = new SelectList(eventQuery, "EventID", "FullName", selectedEvent);
        }

    }
}
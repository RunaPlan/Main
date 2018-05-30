using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using PagedList;

using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Principal;

using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using RAPSimple.Models;
using RAPSimple.DAL;

namespace RAPSimple.Controllers
{
    public class EventController : Controller
    {
        private RAPDbContext db = new RAPDbContext();
        public int ParentId;  // profile ID

        // GET: Event
#if false
        public ActionResult Index()
        {
            //           sParentId = s;
            return View();
        }
        private ActionResult Index(int s)
        {
 //           sParentId = s;
            return View();
        }
#endif
#if true
        //       [ChildActionOnly]
        public ActionResult Index(string sid)
        {
            if (null != sid)
            {
                //              ParentId = int.Parse(sid);
                Session["ParentID"] = sid;
            }
            string sortOrder = "";
            string currentFilter = "";
            string searchString = "";
            int? page = null;
            //            var model = (Profile)ControllerContext.ParentActionViewContext.ViewData.Model;
            //           var p =  User.Identity.GetUserId();
            //            ViewBag.ParentID = ParentId;
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

            var events = from s in db.Events
                         select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                events = events.Where(s => s.Title.Contains(searchString)
                                       || s.Description.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    events = events.OrderByDescending(s => s.Title);
                    break;
#if false
                case "Date":
                    events = events.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    events = events.OrderByDescending(s => s.EnrollmentDate);
                    break;
#endif
                default:  // Name ascending 
                    events = events.OrderBy(s => s.Title);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(events.ToPagedList(pageNumber, pageSize));
        }
#endif

        // GET: events/Create
        [AllowAnonymous]
        public ActionResult Create(int? parentID)
        {
            // more checks here

            //      ViewBag.ParentID = parentID;
            //          Event ev = new Event();
            //          Profile Profile = db.Profiles.Find(parentID);
            //          ev.Profile = Profile;
            //         ev.ProfileId = (int)parentID;
            //         ev.Description = "just a test";
            //            return View(ev);
            return View();
        }

        // POST: events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create([Bind(Include = "Title, StartDate, EndDate, Description, AvgBudget,Location, Difficulties, MaxGroup, Profile, ProfileId, InvitationList")]Event events, HttpPostedFileBase upload)
        {
            try
            {
                if (ModelState.IsValid)
                {
#if false
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
                        events.Files = new List<File> { avatar };
                    }
#endif

                    //                    int id = int.Parse(ViewBag.ID);
                    // find parent profile
                    events.ProfileId = int.Parse(Session["ParentID"].ToString());// id;
                    //                    events.ProfileId = ParentId;// int.Parse(Session["ParentID"].ToString());// id;
                    db.Events.Add(events);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(events);
        }

        public ActionResult Invite(int ? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Event = db.Events.Find(id);
            if (Event == null)
            {
                return HttpNotFound();
            }
            return View(Event);
        }

        // POST:
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Invite")]
//        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InviteAction(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var EventToUpdate = db.Events.Find(id);
            if (TryUpdateModel(EventToUpdate, "",
               new string[] { "InvitationList" }))
            {

                db.Entry(EventToUpdate).State = EntityState.Modified;
                // parse field
                string sInv = EventToUpdate.InvitationList;
                string[] sA = sInv.Split(';');
                int eventID = EventToUpdate.ID; // current event id
                foreach (string s in sA)
                {
                    var prf = from s1 in db.Profiles
                                   where s1.Email.Equals(s)
                                   select s1;
                    if (prf.Count() == 0)
                    {
                        // sent invitation email here!!!!
                        continue;
                    }
                    else
                    {
                        // check if invitation exists
                        int? tstID = prf.First().ID;
                        var invExists = from s1 in db.Invites
                                        where (s1.EventId == eventID && s1.ProfileId == tstID)
                                        select s1;
                        tstID = invExists.Count();
                        if (invExists.Count() == 0)
                        {
                            Invite inv = new Invite();
                            inv.ProfileId = prf.First().ID;
                            inv.EventId = eventID;
                            db.Invites.Add(inv);
                        }
                    }
#if false
                    var prf = db.Profiles.Where(s1 => s1.Email == s).Take(1);
                    if (null != prf)
                    {
                        Invite inv = new Invite();
                        inv.ProfileId = prf.ID;
                        inv.EventId = eventID;
                        db.Invites.Add(inv);

                    }
                    else
                    {
                        // sent invitation email here!!!!
                    }
#endif
                }
                db.SaveChanges();

                return RedirectToAction("Index");

            }

            return View();
        }


    }
}
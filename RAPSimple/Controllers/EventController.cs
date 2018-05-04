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
            string sortOrder="";
            string currentFilter="";
            string searchString="";
            int? page=null;
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
        public ActionResult Create([Bind(Include = "Title, StartDate, EndDate, Description, AvgBudget,Location, Difficulties, MaxGroup, Profile, ProfileId")]Event events, HttpPostedFileBase upload)
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


    }
}
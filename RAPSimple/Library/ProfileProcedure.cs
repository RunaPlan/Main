using RAPSimple.DAL;
using RAPSimple.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace RAPSimple.Library
{
    public static class ProfileProcedure
    {
        private static RAPDbContext db = new RAPDbContext();

        public static string getCurentUserMail()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().
                                   FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            return user.Email;
        }
        
        public static Profile getCurrentProfile()
        {
            string mail = ProfileProcedure.getCurentUserMail();
            return db.Profiles.Where(x => x.Email == mail).First();
        }

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
                return Profiles.First().ID.ToString();//.f.ElementAt(0).ID.ToString();
            }
        }

    }
}
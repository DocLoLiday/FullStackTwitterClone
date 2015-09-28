using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterClone.Models;
using System.Data.SqlClient;
using System.Data;

namespace TwitterClone.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //GET: /Home/Timeline/
        public ActionResult Timeline()
        {
            ViewBag.Message = "Your timeline.";
            return View();
        }

        // POST: /Home/Timeline/:id
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Timeline(TimelineViewModel model)
        {
            if (ModelState.IsValid)
            {
                var conn = new SqlConnection(ConnString.ConnectionString);

                var cmd = new SqlCommand("dbo.AddTweet", conn);
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserID;
                    cmd.Parameters.Add("@TweetText", SqlDbType.NVarChar, 140).Value = model.TweetText;
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }

                return Redirect("/#/timeline/" + model.UserID.ToString());


            }

            
            return Redirect("/#/timeline/" + model.UserID.ToString());
        }

        //GET: /Home/Profile/
        public ActionResult UserProfile()
        {
            ViewBag.Message = "Your timeline.";
            return View();
        }

        // POST: /Home/Profile/:id
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile(UserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var conn = new SqlConnection(ConnString.ConnectionString);

                var cmd = new SqlCommand("dbo.FollowUser", conn);
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = CurrentUser.CurrentUserID;
                    cmd.Parameters.Add("@FollowingID", SqlDbType.Int).Value = model.UserID;
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }

                return Redirect("/#/profile/" + model.UserID.ToString());


            }


            return Redirect("/#/profile/" + model.UserID.ToString());
        }


    }

    
}
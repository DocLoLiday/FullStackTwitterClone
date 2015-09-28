using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TwitterClone.Models;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Web.Script.Serialization;


namespace TwitterClone.Controllers
{
    [Authorize]
    public class AppController : Controller
    {
        

        public AppController()
        {
        }

        

        //
        // GET: /App/Timeline/:id
        [AllowAnonymous]
        public JsonResult Timeline(int id)
        {
            
            var conn = new SqlConnection(ConnString.ConnectionString);
            var cmd = new SqlCommand("dbo.GetTimeline", conn);
            
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = id;

            DataTable table = new DataTable();
            var da = new SqlDataAdapter(cmd);

            da.Fill(table);

            var tl = new TimelineModel();
            var user = new UserModel();
            tl.User = user;

            if (table != null)
            {
                if (table.Rows.Count > 0)
                {
                    tl.User.UserID = id;
                    tl.User.UserName = table.Rows[0][4].ToString();
                    if (table.Rows[0][0].ToString() != ""){
                        List<TweetModel> TweetList = new List<TweetModel>();
                        foreach (DataRow dr in table.Rows)
                        {
                            var Tweet = new TweetModel();
                            var follower = new UserModel();
                            follower.UserID = Convert.ToInt32(dr["FollowerID"]);
                            follower.UserName = dr["FollowerName"].ToString();

                            Tweet.User = follower;
                            Tweet.TweetText = dr["TweetText"].ToString();
                            Tweet.TweetDate = dr["TweetDate"].ToString();
                            TweetList.Add(Tweet);
                        }
                        tl.Tweets = TweetList.ToArray();
                    }
                    
                }
                else
                {
                    tl.User.UserID = CurrentUser.CurrentUserID;
                    tl.User.UserName = CurrentUser.CurrentUserName;
                }
                
            }
            else
            {
                tl.User.UserID = CurrentUser.CurrentUserID;
                tl.User.UserName = CurrentUser.CurrentUserName;
            }
            var json = new JavaScriptSerializer().Serialize(tl);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        // GET: /App/Profile/:id
        [AllowAnonymous]
        public JsonResult UserProfile(int id)
        {
            var conn = new SqlConnection(ConnString.ConnectionString);
            var cmd = new SqlCommand("dbo.GetProfileTweets", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = id;

            DataTable table = new DataTable();
            var da = new SqlDataAdapter(cmd);
                        
            var cmd2 = new SqlCommand("dbo.GetProfileFollowers", conn);

            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.Add("@UserID", SqlDbType.Int).Value = id;

            DataTable table2 = new DataTable();
            var da2 = new SqlDataAdapter(cmd2);

            da.Fill(table);
            da2.Fill(table2);

            var prf = new UserProfileModel();
            var user = new UserModel();
            prf.User = user;

            if (table != null)
            {
                if (table.Rows.Count > 0)
                {
                    prf.User.UserID = id;
                    prf.User.UserName = table.Rows[0][2].ToString();
                    if (table.Rows[0][0].ToString() != "")
                    {
                        List<TweetModel> TweetList = new List<TweetModel>();
                        foreach (DataRow dr in table.Rows)
                        {
                            var Tweet = new TweetModel();

                            Tweet.User = prf.User;
                            Tweet.TweetText = dr["TweetText"].ToString();
                            Tweet.TweetDate = dr["TweetDate"].ToString();
                            TweetList.Add(Tweet);
                        }
                        prf.Tweets = TweetList.ToArray();
                    }

                }

            }
            if (table2 != null)
            {
                if (table.Rows.Count > 0)
                {
                    List<UserModel> FollowerList = new List<UserModel>();
                    foreach (DataRow dr in table2.Rows)
                    {
                        var Follower = new UserModel();

                        Follower.UserID = Convert.ToInt32(dr["UserID"]);
                        Follower.UserName = dr["UserName"].ToString();
                        FollowerList.Add(Follower);
                    }
                    prf.Followers = FollowerList.ToArray();
                }
            }
            var json = new JavaScriptSerializer().Serialize(prf);
            return Json(json, JsonRequestBehavior.AllowGet);
        }






    }
}
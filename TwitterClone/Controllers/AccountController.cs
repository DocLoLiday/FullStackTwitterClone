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

namespace TwitterClone.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        
        public AccountController()
        {
        }

        
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var conn = new SqlConnection(ConnString.ConnectionString);

                var cmd = new SqlCommand("dbo.AuthenticateUser", conn);

                int UserId = 0;

                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 15).Value = model.UserName;
                    cmd.Parameters.Add("@PWord", SqlDbType.NVarChar, 30).Value = model.Password;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    if (!System.DBNull.Value.Equals(cmd.Parameters["@UserID"].Value))
                    {
                        UserId = Convert.ToInt32(cmd.Parameters["@UserID"].Value);
                        CurrentUser.CurrentUserID = UserId;
                        CurrentUser.CurrentUserName = model.UserName;
                    }
                    
                    
                }
                finally
                {
                    conn.Close();
                }
                //Brings you to your timeline
                if (UserId != 0)
                {
                    return Redirect("/#/timeline/" + UserId.ToString());
                }
                else
                {
                    return Redirect("/#");
                }
                

            }

            // If we got this far, something failed, redisplay form
            return Redirect("/#");

        }
        

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var conn = new SqlConnection(ConnString.ConnectionString);

                var cmd = new SqlCommand("dbo.AddUser", conn);
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 15).Value = model.UserName;
                    cmd.Parameters.Add("@PWord", SqlDbType.NVarChar, 30).Value = model.Password;
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }                            
                
                return Redirect("/#");
                                
            }

            // If we got this far, something failed, redisplay form
            return Redirect("/#/register");
        }
        
        

      

        
    }
}
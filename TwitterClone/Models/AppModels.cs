using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitterClone.Models
{

    public class UserModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }

    public class TweetModel
    {
        public UserModel User { get; set; }
        public string TweetText { get; set; }
        public string TweetDate { get; set; }
    }

    public class TimelineViewModel
    {
        public int UserID { get; set; }
        public string TweetText { get; set; }
    }

    public class TimelineModel
    {
        public UserModel User { get; set; }
        public TweetModel[] Tweets { get; set; }
    }

    public class UserProfileViewModel
    {
        public int UserID { get; set; }
    }

    public class UserProfileModel : TimelineModel
    {
        public UserModel[] Followers { get; set; }
    }

    public static class CurrentUser
    {
        public static int CurrentUserID { get; set; }
        public static string CurrentUserName { get; set; }
        
    }

    public static class ConnString
    {
        public const string ConnectionString = "Server=DONSZECHUANPC\\SQLEXPRESS;DataBase=TwitterClone;Integrated Security=True";
    }
}

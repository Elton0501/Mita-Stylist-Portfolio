using DataBase;
using DataModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Services
{
    public class UserService
    {
        #region singleton
        public static UserService Instance
        {
            get
            {
                if (instance == null) instance = new UserService();
                return instance;
            }
        }
        private static UserService instance { get; set; }

        public UserService()
        {
        }
        #endregion

        public IEnumerable<User> GetUsers()
        {
            var data = new List<User>();
            using (var context = new ApplicationDbContext())
            {
                data = context.User.OrderByDescending(x => x.Id).ToList();
                return data;
            }
        }


        public string GetCurrentUserLogin()
        {
            string data = null;
            HttpCookie WebCookie = HttpContext.Current.Request.Cookies[Constant.WebCookie];
            if (WebCookie != null)
            {
                data = HelperService.Instance.Decrypt(WebCookie.Value.ToString());
            }
            return data;
        }
        public string GetCurrentUserName(string email)
        {
            string data = null;
            using (var context = new ApplicationDbContext())
            {
                var getdata = context.User.FirstOrDefault(x => x.Email == email);
                data = getdata != null ? getdata.Name != "" && getdata.Name != null ? getdata.Name : email : null;
            }
            return data;
        }
        public User GetUser(string loginUser)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.User.FirstOrDefault(x => x.Email == loginUser);
            }
        }

        internal User GetUserByUID(int userId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.User.FirstOrDefault(x => x.Id == userId);
            }
        }

        public void RemoveUser(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var AdminRole = context.Roles.FirstOrDefault(x => x.RoleName == Constant.Role.Admin);
                var data = context.User.FirstOrDefault(x => x.Id == id);
                if (data.RoleId != AdminRole.RoleId)
                {
                    context.User.Remove(data);
                    context.SaveChanges();
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class Constant
    {
        public const string WebCookie = "WebID";
        public static class Role
        {
            public const string Admin = "Admin";
        }
        public enum ImageType
        {
            Portrait = 0,
            Landscape = 1,
        }
    }
}
